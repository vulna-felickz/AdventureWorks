﻿using AutoFixture.Xunit2;
using AW.Services.Customer.Core.Handlers.DeleteIndividualCustomerPhone;
using AW.Services.Customer.Core.Specifications;
using AW.SharedKernel.Interfaces;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AW.Services.Customer.Core.UnitTests
{
    public class DeleteIndividualCustomerPhoneCommandUnitTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ExistingCustomerAndAddress_DeleteCustomerAddress(
            [Frozen] Mock<IRepository<Entities.IndividualCustomer>> customerRepoMock,
            Entities.IndividualCustomer customer,
            DeleteIndividualCustomerPhoneCommandHandler sut,
            DeleteIndividualCustomerPhoneCommand command
        )
        {
            //Arrange
            customer.Person.PhoneNumbers = new List<Entities.PersonPhone>
            {
                new Entities.PersonPhone
                {
                    PhoneNumberType = command.Phone.PhoneNumberType,
                    PhoneNumber = command.Phone.PhoneNumber
                }
            };

            customerRepoMock.Setup(x => x.GetBySpecAsync(
                It.IsAny<GetIndividualCustomerSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(customer);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            customerRepoMock.Verify(x => x.UpdateAsync(
                It.IsAny<Entities.IndividualCustomer>(),
                It.IsAny<CancellationToken>()
            ));
        }

        [Theory]
        [AutoMoqData]
        public void Handle_CustomerDoesNotExist_ThrowArgumentNullException(
            [Frozen] Mock<IRepository<Entities.IndividualCustomer>> customerRepoMock,
            DeleteIndividualCustomerPhoneCommandHandler sut,
            DeleteIndividualCustomerPhoneCommand command
        )
        {
            // Arrange
            customerRepoMock.Setup(x => x.GetBySpecAsync(
                It.IsAny<GetIndividualCustomerSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Entities.IndividualCustomer)null);

            //Act
            Func<Task> func = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'individualCustomer')");
        }

        [Theory]
        [AutoMoqData]
        public void Handle_PhoneNumberDoesNotExist_ThrowArgumentNullException(
            DeleteIndividualCustomerPhoneCommandHandler sut,
            DeleteIndividualCustomerPhoneCommand command
        )
        {
            //Act
            Func<Task> func = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            func.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'phone')");
        }
    }
}