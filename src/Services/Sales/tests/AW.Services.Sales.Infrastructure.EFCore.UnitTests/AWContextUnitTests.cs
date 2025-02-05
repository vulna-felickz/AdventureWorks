﻿using AW.Services.Infrastructure.EventBus.IntegrationEventLog;
using AW.Services.Sales.Core.Entities;
using AW.Services.Sales.Infrastructure.EFCore.Configurations;
using AW.Services.SharedKernel.EFCore;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AW.Services.Sales.Infrastructure.EFCore.UnitTests
{
    public class AWContextUnitTests
    {
        [Theory, AutoMoqData]
        public void CreateDatabase_ModelConfigurationsAreApplied(
            Mock<IMediator> mockMediator,
            Mock<ILogger<AWContext>> mockLogger
        )
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AWContext(
                mockLogger.Object,
                contextOptions,
                mockMediator.Object,
                typeof(SalesOrderConfiguration).Assembly
            );

            //Act
            context.Database.EnsureCreated();

            //Assert
            var entityTypes = context.Model.GetEntityTypes().ToList();
            entityTypes[0].Name.Should().Be(typeof(IntegrationEventLogEntry).FullName);
            entityTypes[1].Name.Should().Be(typeof(CreditCard).FullName);
            entityTypes[2].Name.Should().Be(typeof(Customer).FullName);
            entityTypes[3].Name.Should().Be(typeof(IndividualCustomer).FullName);
            entityTypes[4].Name.Should().Be(typeof(Person).FullName);
            entityTypes[5].Name.Should().Be(typeof(SalesOrder).FullName);
            entityTypes[6].Name.Should().Be($"{typeof(SalesOrder).FullName}.BillToAddress#Address");
            entityTypes[7].Name.Should().Be($"{typeof(SalesOrder).FullName}.ShipToAddress#Address");
            entityTypes[8].Name.Should().Be(typeof(SalesOrderLine).FullName);
            entityTypes[9].Name.Should().Be(typeof(SalesOrderSalesReason).FullName);
            entityTypes[10].Name.Should().Be(typeof(SalesPerson).FullName);
            entityTypes[11].Name.Should().Be(typeof(SalesPersonEmailAddress).FullName);
            entityTypes[12].Name.Should().Be(typeof(SalesPersonPhone).FullName);
            entityTypes[13].Name.Should().Be(typeof(SalesReason).FullName);
            entityTypes[14].Name.Should().Be(typeof(SpecialOffer).FullName);
            entityTypes[15].Name.Should().Be(typeof(SpecialOfferProduct).FullName);
            entityTypes[16].Name.Should().Be(typeof(StoreCustomer).FullName);
        }
    }
}