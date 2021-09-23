﻿using AutoFixture.Xunit2;
using AW.Services.Customer.Core.Handlers.DeleteCustomer;
using AW.Services.Customer.Core.Specifications;
using AW.SharedKernel.Interfaces;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AW.Services.Customer.Core.UnitTests
{
    public class DeleteCustomerCommandUnitTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ExistingCustomer_DeleteCustomer(
            [Frozen] Mock<IRepository<Entities.Customer>> customerRepoMock,
            DeleteCustomerCommandHandler sut,
            DeleteCustomerCommand command
        )
        {
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            customerRepoMock.Verify(x => x.DeleteAsync(
                It.IsAny<Entities.Customer>(),
                It.IsAny<CancellationToken>()    
            ));
        }

        [Theory]
        [AutoMoqData]
        public void Handle_CustomerDoesNotExist_ThrowArgumentNullException(
            [Frozen] Mock<IRepository<Entities.Customer>> customerRepoMock,
            DeleteCustomerCommandHandler sut,
            DeleteCustomerCommand command
        )
        {
            // Arrange
            customerRepoMock.Setup(x => x.GetBySpecAsync(
                It.IsAny<GetCustomerSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Entities.Customer)null);

            //Act
            Func<Task> func = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'customer')");
        }
    }
}