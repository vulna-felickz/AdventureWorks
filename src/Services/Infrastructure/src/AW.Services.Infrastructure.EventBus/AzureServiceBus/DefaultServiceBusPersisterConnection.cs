﻿using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;
using System.Threading.Tasks;

namespace AW.Services.Infrastructure.EventBus.AzureServiceBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private readonly string connectionString;
        private readonly ServiceBusAdministrationClient adminClient;
        private ServiceBusClient serviceBusClient;
        private readonly ServiceBusProcessor processor;

        public DefaultServiceBusPersisterConnection(string connectionString, string topicName)
        {
            this.connectionString = connectionString;
            adminClient = new ServiceBusAdministrationClient(connectionString);
            serviceBusClient = new ServiceBusClient(connectionString);

            processor = serviceBusClient.CreateProcessor(
                topicName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false,
                    MaxConcurrentCalls = 10
                }
                );
        }

        public ServiceBusClient ServiceBusClient
        {
            get
            {
                if (serviceBusClient.IsClosed)
                {
                    serviceBusClient = new ServiceBusClient(connectionString);
                }
                return serviceBusClient;
            }
        }

        public ServiceBusAdministrationClient AdminClient
        {
            get
            {
                return adminClient;
            }
        }

        public ServiceBusProcessor Processor
        {
            get { return processor; }
        }

        public async Task StartReceiving()
        {
            await processor.StartProcessingAsync();
        }
        public async Task StopReceiving()
        {
            await processor.StopProcessingAsync();
        }

        public ServiceBusClient CreateModel()
        {
            return ServiceBusClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Task.Run(() => serviceBusClient?.DisposeAsync()).Wait();
            Task.Run(() => processor?.DisposeAsync()).Wait();
        }
    }
}