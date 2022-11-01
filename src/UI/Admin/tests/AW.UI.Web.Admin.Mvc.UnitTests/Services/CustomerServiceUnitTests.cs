﻿using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using AW.UI.Web.Admin.Mvc.Services;
using AW.UI.Web.Admin.Mvc.ViewModels.Customer;
using AW.SharedKernel.Interfaces;
using System.Threading.Tasks;
using AW.SharedKernel.UnitTesting;
using AutoFixture.Xunit2;
using AW.UI.Web.SharedKernel.Interfaces.Api;
using AW.UI.Web.SharedKernel.ReferenceData.Handlers.GetTerritories;
using MediatR;
using System.Threading;
using AW.UI.Web.SharedKernel.ReferenceData.Handlers.GetStatesProvinces;
using AW.UI.Web.SharedKernel.ReferenceData.Handlers.GetContactTypes;
using AW.UI.Web.SharedKernel.SalesPerson.Handlers.GetSalesPersons;
using AW.UI.Web.SharedKernel.Customer.Handlers.GetCustomers;
using AW.UI.Web.SharedKernel.Customer.Handlers.UpdateCustomer;
using AW.UI.Web.SharedKernel.Customer.Handlers.GetCustomer;
using AW.UI.Web.SharedKernel.Customer.Handlers.GetStoreCustomer;
using AW.UI.Web.SharedKernel.Customer.Handlers.GetIndividualCustomer;
using System;

namespace AW.UI.Web.Admin.Mvc.UnitTests.Services
{
    public class CustomerServiceUnitTests
    {
        public class GetCustomers
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetCustomers_FirstPage_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                List<Territory> territories
            )
            {
                //Arrange
                var customers = Enumerable.Repeat(new SharedKernel.Customer.Handlers.GetCustomers.StoreCustomer(), 10).ToList();

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomersQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(new GetCustomersResponse
                {
                    Customers = customers.Cast<SharedKernel.Customer.Handlers.GetCustomers.Customer>().ToList(),
                    TotalCustomers = customers.Count * 10
                });

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetTerritoriesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(territories);

                //Act
                var viewModel = await sut.GetCustomers(0, 10, null, CustomerType.Store, null);

                //Assert
                viewModel.Customers.Count.Should().Be(10);
                viewModel.Territories.ToList().Count.Should().Be(4);
                viewModel.Territories.ToList()[0].Text.Should().Be("All");
                viewModel.CustomerTypes.Count().Should().Be(3);
                viewModel.PaginationInfo.Should().NotBeNull();
                viewModel.PaginationInfo.ActualPage.Should().Be(0);
                viewModel.PaginationInfo.ItemsPerPage.Should().Be(10);
                viewModel.PaginationInfo.TotalItems.Should().Be(100);
                viewModel.PaginationInfo.TotalPages.Should().Be(10);
                viewModel.PaginationInfo.Next.Should().Be("");
                viewModel.PaginationInfo.Previous.Should().Be("disabled");
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetCustomers_LastPage_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                List<Territory> territories
            )
            {
                //Arrange
                var customers = Enumerable.Repeat(new SharedKernel.Customer.Handlers.GetCustomers.StoreCustomer(), 10).ToList();

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomersQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(new GetCustomersResponse
                {
                    Customers = customers.Cast<SharedKernel.Customer.Handlers.GetCustomers.Customer>().ToList(),
                    TotalCustomers = customers.Count * 10
                });

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetTerritoriesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(territories);

                //Act
                var viewModel = await sut.GetCustomers(9, 10, null, CustomerType.Store, null);

                //Assert
                viewModel.Customers.Count.Should().Be(10);
                viewModel.Territories.ToList().Count.Should().Be(4);
                viewModel.Territories.ToList()[0].Text.Should().Be("All");
                viewModel.CustomerTypes.Count().Should().Be(3);
                viewModel.PaginationInfo.Should().NotBeNull();
                viewModel.PaginationInfo.ActualPage.Should().Be(9);
                viewModel.PaginationInfo.ItemsPerPage.Should().Be(10);
                viewModel.PaginationInfo.TotalItems.Should().Be(100);
                viewModel.PaginationInfo.TotalPages.Should().Be(10);
                viewModel.PaginationInfo.Next.Should().Be("disabled");
                viewModel.PaginationInfo.Previous.Should().Be("");
            }
        }

        public class GetCustomer
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetCustomer_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomer(customer.AccountNumber);

                //Assert
                viewModel.Customer.AccountNumber.Should().Be(customer.AccountNumber);

                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class GetStoreCustomerForEdit
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetStoreCustomerForEdit_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer,
                List<Territory> territories,
                List<SalesPerson> salesPersons
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .ReturnsAsync(customer);

                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTerritoriesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(territories);

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetSalesPersonsQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(salesPersons);

                //Act
                var viewModel = await sut.GetStoreCustomerForEdit(customer.AccountNumber);

                //Assert
                viewModel.Customer.AccountNumber.Should().Be(customer.AccountNumber);
                viewModel.Territories.ToList().Count.Should().Be(4);
                viewModel.Territories.ToList()[0].Text.Should().Be("--Select--");
                viewModel.SalesPersons.ToList().Count.Should().Be(4);
                viewModel.SalesPersons.ToList()[0].Text.Should().Be("All");
            }
        }

        public class GetIndividualCustomerForEdit
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetIndividualCustomerForEdit_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                List<Territory> territories,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetIndividualCustomer.IndividualCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetIndividualCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .ReturnsAsync(customer);

                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetTerritoriesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(territories);

                //Act
                var viewModel = await sut.GetIndividualCustomerForEdit(customer.AccountNumber);

                //Assert
                viewModel.Customer.AccountNumber.Should().Be(customer.AccountNumber);
                viewModel.Territories.ToList().Count.Should().Be(4);
                viewModel.Territories.ToList()[0].Text.Should().Be("--Select--");
                viewModel.EmailPromotions.Count().Should().Be(4);
                viewModel.EmailPromotions.ToList()[0].Text.Should().Be("All");
            }
        }

        public class UpdateStore
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateStore_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                StoreCustomerViewModel viewModel,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .ReturnsAsync(customer);

                //Act
                await sut.UpdateStore(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateStore_WithoutViewModel_ThrowsArgumentNullException(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut
            )
            {
                //Arrange

                //Act
                Func<Task> func = async () => await sut.UpdateStore(null);

                //Assert
                await func.Should().ThrowAsync<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'viewModel')");

                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    ),
                    Times.Never
                );
            }
        }

        public class UpdateIndividual
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateIndividual_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                IndividualCustomerViewModel viewModel
            )
            {
                //Arrange

                //Act
                await sut.UpdateIndividual(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateIndividual_WithoutViewModel_ThrowsArgumentNullException(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut
            )
            {
                //Arrange

                //Act
                Func<Task> func = async () => await sut.UpdateIndividual(null);

                //Assert
                await func.Should().ThrowAsync<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'viewModel')");

                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    ),
                    Times.Never
                );
            }
        }

        public class AddAddress
        {
            [Theory, AutoMoqData]
            public void AddAddress_ReturnsViewModel(
                CustomerService sut,
                string accountNumber,
                string customerName
            )
            {
                //Arrange

                //Act
                var viewModel = sut.AddAddress(accountNumber, customerName);

                //Assert
                viewModel.IsNewAddress.Should().Be(true);
                viewModel.AccountNumber.Should().Be(accountNumber);
                viewModel.CustomerName.Should().Be(customerName);
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task AddAddress_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditCustomerAddressViewModel viewModel,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.AddAddress(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class GetCustomerAddress
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetCustomerAddress_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomerAddress(
                    customer.AccountNumber,
                    customer.Addresses[0].AddressType
                );

                //Assert
                viewModel.AccountNumber.Should().Be(customer.AccountNumber);
                viewModel.CustomerName.Should().Be(customer.Name);
                viewModel.CustomerAddress.Should().NotBeNull();
            }
        }

        public class UpdateAddress
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateAddress_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditCustomerAddressViewModel viewModel,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                viewModel.CustomerAddress.AddressType = customer.Addresses[0].AddressType;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.UpdateAddress(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateAddress_CustomerNotFound_ThrowsArgumentNullException(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditCustomerAddressViewModel viewModel,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                viewModel.CustomerAddress.AddressType = customer.Addresses[0].AddressType;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(null as SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer);

                //Act

                Func<Task> func = async () => await sut.UpdateAddress(viewModel);

                //Assert
                await func.Should().ThrowAsync<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'customer')");

                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateAddress_AddressNotFound_ThrowsArgumentNullException(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditCustomerAddressViewModel viewModel,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act

                Func<Task> func = async () => await sut.UpdateAddress(viewModel);

                //Assert
                await func.Should().ThrowAsync<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'addressToUpdate')");

                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class GetCustomerAddressForDelete
        {
            [Theory, AutoMoqData]
            public async Task GetCustomerAddressForDelete_Store_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                    It.IsAny<GetCustomerQuery>(),
                    It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomerAddressForDelete(customer.AccountNumber, "Main Office");

                //Assert
                viewModel.CustomerName.Should().Be(customer.Name);
            }

            [Theory, AutoMoqData]
            public async Task GetCustomerAddressForDelete_Person_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetCustomer.IndividualCustomer customer
            )
            {
                //Arrange
                mockMediator
                    .Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomerAddressForDelete(customer.AccountNumber, "Home");

                //Assert
                viewModel.CustomerName.Should().Be(customer.CustomerName);
            }
        }

        public class GetStatesProvinces
        {
            [Theory, AutoMoqData]
            public async Task GetStatesProvinces_ReturnsList(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                List<StateProvince> statesProvinces,
                string countryRegionCode
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStatesProvincesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(statesProvinces);

                //Act
                var selectListItems = await sut.GetStatesProvinces(countryRegionCode);
                var list = selectListItems.ToList();

                //Assert
                list.Count.Should().Be(4);
                list[0].Value.Should().Be("");
                list[0].Text.Should().Be("--Select--");
            }
        }

        public class GetStatesProvincesJson
        {
            [Theory, AutoMoqData]
            public async Task GetStatesProvincesJson_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                string countryRegionCode,
                List<StateProvince> statesProvinces
            )
            {
                //Arrange
                mockMediator.Setup(x => x.Send(
                        It.IsAny<GetStatesProvincesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(statesProvinces);

                //Act
                var viewModel = await sut.GetStatesProvincesJson(countryRegionCode);

                //Assert
                viewModel.Should().BeEquivalentTo(statesProvinces);
            }
        }

        public class DeleteAddress
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task DeleteAddress_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetCustomer.IndividualCustomer customer
            )
            {
                //Arrange
                mockMediator
                    .Setup(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
               )
               .ReturnsAsync(customer);

                //Act
                await sut.DeleteAddress(
                    customer.AccountNumber,
                    customer.Addresses[0].AddressType
                );

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class AddContact
        {
            [Theory, AutoMoqData]
            public async Task AddContact_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                List<ContactType> contactTypes,
                string accountNumber,
                string customerName
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetContactTypesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(contactTypes);

                //Act
                var viewModel = await sut.AddContact(accountNumber, customerName);

                //Assert
                viewModel.IsNewContact.Should().Be(true);
                viewModel.ContactTypes.Count().Should().Be(4);
                viewModel.ContactTypes.ToList()[0].Text.Should().Be("--Select--");
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task AddContact_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditCustomerContactViewModel viewModel,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.AddContact(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class GetCustomerContact
        {
            [Theory, AutoMoqData]
            public async Task GetCustomerContact_ReturnsViewModel(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                List<ContactType> contactTypes,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer,
                string contactName,
                string contactType
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetContactTypesQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(contactTypes);

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomerContact(customer.AccountNumber, contactName, contactType);

                //Assert            
                viewModel.IsNewContact.Should().Be(false);
                viewModel.ContactTypes.Count().Should().Be(4);
                viewModel.ContactTypes.ToList()[0].Text.Should().Be("--Select--");
            }
        }

        public class UpdateContact
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task UpdateContact_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer,
                EditCustomerContactViewModel viewModel
            )
            {
                //Arrange
                viewModel.CustomerContact.ContactType = customer.Contacts[0].ContactType;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.UpdateContact(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<UpdateCustomerCommand>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class GetCustomerContactForDelete
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task GetCustomerContactForDelete_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer
            )
            {
                //Arrange
                mockMediator.Setup(_ => _.Send(
                    It.IsAny<GetStoreCustomerQuery>(),
                    It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                var viewModel = await sut.GetCustomerContactForDelete(
                    customer.AccountNumber,
                    customer.Contacts[0].ContactPerson.Name.FullName,
                    customer.Contacts[0].ContactType
                );

                //Assert
                viewModel.AccountNumber.Should().Be(customer.AccountNumber);
                viewModel.CustomerName.Should().Be(customer.Name);
                viewModel.ContactType.Should().Be(customer.Contacts[0].ContactType);
            }
        }

        public class DeleteContact
        {
            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task DeleteContact_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer,
                DeleteCustomerContactViewModel viewModel
            )
            {
                //Arrange
                viewModel.ContactType = customer.Contacts[0].ContactType;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.DeleteContact(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class AddContactEmailAddress
        {
            [Theory, AutoMoqData]
            public void AddContactEmailAddress_ReturnsViewModel(
                CustomerService sut,
                string accountNumber,
                string customerName
            )
            {
                //Arrange

                //Act
                var viewModel = sut.AddEmailAddress(accountNumber, customerName);

                //Assert
                viewModel.IsNewEmailAddress.Should().Be(true);
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task AddContactEmailAddress_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditEmailAddressViewModel viewModel,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer
            )
            {
                //Arrange
                viewModel.PersonName = customer.Contacts[0].ContactPerson.Name.FullName;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.AddContactEmailAddress(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }

        public class AddContactPhoneNumber
        {
            [Theory, AutoMoqData]
            public void AddContactPhoneNumber_ReturnsViewModel(
                CustomerService sut,
                string accountNumber,
                string personName
            )
            {
                //Arrange

                //Act
                var viewModel = sut.AddPhoneNumber(accountNumber, personName);

                //Assert
                viewModel.IsNewPhoneNumber.Should().Be(true);
            }

            [Theory, AutoMapperData(typeof(MappingProfile))]
            public async Task AddContactPhoneNumber_OK(
                [Frozen] Mock<IMediator> mockMediator,
                CustomerService sut,
                EditPhoneNumberViewModel viewModel,
                SharedKernel.Customer.Handlers.GetStoreCustomer.StoreCustomer customer
            )
            {
                //Arrange
                viewModel.PersonName = customer.Contacts[0].ContactPerson.Name.FullName;

                mockMediator.Setup(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(customer);

                //Act
                await sut.AddContactPhoneNumber(viewModel);

                //Assert
                mockMediator.Verify(_ => _.Send(
                        It.IsAny<GetStoreCustomerQuery>(),
                        It.IsAny<CancellationToken>()
                    )
                );
            }
        }
    }
}