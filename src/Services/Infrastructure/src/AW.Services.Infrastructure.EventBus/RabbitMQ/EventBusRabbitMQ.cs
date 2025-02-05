﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using AW.Services.Infrastructure.EventBus.Abstractions;
using AW.Services.Infrastructure.EventBus.Events;
using AW.Services.Infrastructure.EventBus.Extensions;

namespace AW.Services.Infrastructure.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        const string BROKER_NAME = "event_bus";

        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> logger;
        private readonly IEventBusSubscriptionsManager subsManager;
        private readonly int retryCount;

        private IModel? consumerChannel;
        private string queueName;

        public EventBusRabbitMQ(IServiceScopeFactory serviceScopeFactory,
            IRabbitMQPersistentConnection persistentConnection,
            ILogger<EventBusRabbitMQ> logger,
            IEventBusSubscriptionsManager subsManager,
            string queueName,
            int retryCount = 5)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.persistentConnection = persistentConnection;
            this.logger = logger;
            this.subsManager = subsManager;

            this.queueName = queueName;
            this.retryCount = retryCount;
            subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            using var channel = persistentConnection.CreateModel();
            channel.QueueUnbind(queue: queueName,
                exchange: BROKER_NAME,
                routingKey: eventName);

            if (subsManager.IsEmpty)
            {
                queueName = string.Empty;
                consumerChannel?.Close();
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using var channel = persistentConnection.CreateModel();
            logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            DoInternalSubscription(eventName);
            subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicConsume();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                if (consumerChannel == null)
                    consumerChannel = CreateConsumerChannel();

                consumerChannel.QueueBind(queue: queueName,
                                    exchange: BROKER_NAME,
                                    routingKey: eventName);
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = subsManager.GetEventKey<T>();

            logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (consumerChannel != null)
                {
                    consumerChannel.Dispose();
                }

                subsManager.Clear();
            }
        }

        private void StartBasicConsume()
        {
            logger.LogTrace("Starting RabbitMQ basic consume");

            if (consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(consumerChannel);

                consumer.Received += Consumer_Received;

                consumerChannel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                consumerChannel?.Dispose();
                consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
            if (subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        using var scope = serviceScopeFactory.CreateScope();
                        if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) 
                            continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        using var scope = serviceScopeFactory.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType?.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent! })!;
                    }
                }
            }
            else
            {
                logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
