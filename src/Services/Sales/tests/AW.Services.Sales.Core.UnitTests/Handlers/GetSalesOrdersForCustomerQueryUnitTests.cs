using AutoFixture.Xunit2;
using AW.Services.Sales.Core.AutoMapper;
using AW.Services.Sales.Core.Handlers.GetSalesOrdersForCustomer;
using AW.Services.Sales.Core.Specifications;
using AW.Services.SharedKernel.Interfaces;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AW.Services.Sales.Core.UnitTests.Handlers
{
    public class GetSalesOrdersForCustomerQueryUnitTests
    {
        [Theory, AutoMapperData(typeof(MappingProfile))]
        public async Task Handle_SalesOrderExists_ReturnSalesOrder(
            List<Core.Entities.SalesOrder> salesOrders,
            [Frozen] Mock<IRepository<Core.Entities.SalesOrder>> salesOrderRepoMock,
            GetSalesOrdersForCustomerQueryHandler sut,
            GetSalesOrdersForCustomerQuery query
        )
        {
            //Arrange
            salesOrderRepoMock.Setup(x => x.ListAsync(
                It.IsAny<GetSalesOrdersForCustomerSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(salesOrders);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            salesOrderRepoMock.Verify(x => x.ListAsync(
                It.IsAny<GetSalesOrdersForCustomerSpecification>(),
                It.IsAny<CancellationToken>()
            ));

            result.SalesOrders.Should().BeEquivalentTo(salesOrders,
                opt => opt
                    .Excluding(_ => _.Path.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase))
                    .Excluding(_ => _.Path.EndsWith("DomainEvents", StringComparison.InvariantCultureIgnoreCase))
                    .Excluding(_ => _.Path.EndsWith("SpecialOfferProduct"))
                    .Excluding(_ => _.Path.EndsWith("SalesReason"))
                    .Excluding(_ => _.Path.EndsWith("SalesPerson"))
            );
        }
    }
}