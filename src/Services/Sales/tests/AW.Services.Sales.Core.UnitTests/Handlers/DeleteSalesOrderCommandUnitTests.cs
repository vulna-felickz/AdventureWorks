﻿using AutoFixture.Xunit2;
using AW.Services.Sales.Core.Exceptions;
using AW.Services.Sales.Core.Handlers.DeleteSalesOrder;
using AW.Services.Sales.Core.Specifications;
using AW.Services.SharedKernel.Interfaces;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AW.Services.Sales.Core.UnitTests.Handlers
{
    public class DeleteSalesOrderCommandUnitTests
    {
        [Theory, AutoMoqData()]
        public async Task Handle_SalesOrderExists_DeleteSalesOrder(
            [Frozen] Mock<IRepository<Core.Entities.SalesOrder>> salesOrderRepositoryMock,
            DeleteSalesOrderCommandHandler sut,
            DeleteSalesOrderCommand command
        )
        {
            //Arrange

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);

            salesOrderRepositoryMock.Verify(_ => _.DeleteAsync(
                It.IsAny<Core.Entities.SalesOrder>(),
                It.IsAny<CancellationToken>())
            );
        }

        [Theory, AutoMoqData()]
        public async Task Handle_SalesOrderDoesNotExist_ThrowSalesOrderNotFoundException(
            [Frozen] Mock<IRepository<Core.Entities.SalesOrder>> salesOrderRepositoryMock,
            DeleteSalesOrderCommandHandler sut,
            DeleteSalesOrderCommand command
        )
        {
            //Arrange
            salesOrderRepositoryMock.Setup(_ => _.SingleOrDefaultAsync(
                It.IsAny<GetSalesOrderSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Core.Entities.SalesOrder?)null);

            //Act
            Func<Task> func = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<SalesOrderNotFoundException>();

            salesOrderRepositoryMock.Verify(_ => _.DeleteAsync(
                    It.IsAny<Core.Entities.SalesOrder>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never
            );
        }
    }
}