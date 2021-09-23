﻿using AW.SharedKernel.Api.EventBus.Abstractions;
using System.Threading.Tasks;

namespace AW.SharedKernel.Api.UnitTests
{
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public Task Handle(TestIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}