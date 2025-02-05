﻿using AW.Services.Infrastructure.EventBus.Events;
using AW.Services.Infrastructure.EventBus.IntegrationEventLog;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AW.Services.SharedKernel.EFCore
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly AWContext dbContext;
        private readonly List<Type> eventTypes;

        public IntegrationEventLogService(
            AWContext dbContext,
            Assembly eventTypesAssembly
        )
        {
            this.dbContext = dbContext;

            eventTypes = eventTypesAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId;

            var result = await dbContext.Set<IntegrationEventLogEntry>()
                .Where(e => e.TransactionId == tid && e.State == EventState.NotPublished)
                .ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(eventTypes.Find(t => t.Name == e.EventTypeShortName)!));
            }

            return new List<IntegrationEventLogEntry>();
        }

        public async Task SaveEventAsync(IntegrationEvent @event, Guid transactionId)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event, transactionId);

            await dbContext.Set<IntegrationEventLogEntry>().AddAsync(eventLogEntry);

            await dbContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.PublishedFailed);
        }

        private async Task UpdateEventStatus(Guid eventId, EventState status)
        {
            var eventLogEntry = dbContext.Set<IntegrationEventLogEntry>()
                .Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventState.InProgress)
                eventLogEntry.TimesSent++;

            dbContext.Set<IntegrationEventLogEntry>().Update(eventLogEntry);
            await dbContext.SaveChangesAsync();
        }
    }
}