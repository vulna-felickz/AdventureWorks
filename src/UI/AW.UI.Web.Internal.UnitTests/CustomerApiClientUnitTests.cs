﻿using AW.UI.Web.Internal.ApiClients.CustomerApi;
using AW.UI.Web.Internal.ApiClients.CustomerApi.Models.GetCustomers;
using AW.UI.Web.Internal.JsonConverters;
using AW.UI.Web.Internal.UnitTests.TestBuilders.GetCustomers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace AW.UI.Web.Internal.UnitTests
{
    public class CustomerApiClientUnitTests
    {
        public class GetCustomers
        {
            [Fact]
            public async void GetCustomers_CustomersFound_ReturnsCustomer()
            {
                //Arrange

                var mockLogger = new Mock<ILogger<CustomerApiClient>>();

                var customers = new ApiClients.CustomerApi.Models.GetCustomers.GetCustomersResponse
                {
                    Customers = new List<ApiClients.CustomerApi.Models.GetCustomers.Customer>
                {
                    new StoreCustomerBuilder()
                        .WithTestValues()
                        .Build()
                },
                    TotalCustomers = 1
                };

                var httpClient = new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(
                            JsonSerializer.Serialize(customers, new JsonSerializerOptions
                            {
                                Converters =
                                {
                                    new JsonStringEnumConverter(),
                                    new CustomerConverter<
                                        ApiClients.CustomerApi.Models.GetCustomers.Customer,
                                        ApiClients.CustomerApi.Models.GetCustomers.StoreCustomer,
                                        ApiClients.CustomerApi.Models.GetCustomers.IndividualCustomer>()
                                },
                                IgnoreReadOnlyProperties = true,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            }),
                            Encoding.UTF8,
                            "application/json"
                        )
                    };

                    return await Task.FromResult(responseMessage);
                }))
                {
                    BaseAddress = new Uri("http://baseaddress")
                };

                //Act
                var sut = new CustomerApiClient(httpClient, mockLogger.Object);
                var response = await sut.GetCustomersAsync(0, 10, null, null, null);

                //Assert
                response.Should().NotBeNull();
                response.TotalCustomers.Should().Be(1);
                response.Customers[0].AccountNumber.Should().Be("AW00000001");
                response.Customers[0].Addresses.Count.Should().Be(1);

                (response.Customers[0] as StoreCustomer).Name.Should().Be("A Bike Store");
                (response.Customers[0] as StoreCustomer).SalesPerson.Should().Be("Pamela O Ansman-Wolfe");
                (response.Customers[0] as StoreCustomer).Territory.Should().Be("Northwest");

            }

            [Fact]
            public void GetCustomers_NoCustomersFound_ThrowsHttpRequestException()
            {
                //Arrange

                var mockLogger = new Mock<ILogger<CustomerApiClient>>();

                var httpClient = new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                    return await Task.FromResult(responseMessage);
                }))
                {
                    BaseAddress = new Uri("http://baseaddress")
                };

                //Act
                var sut = new CustomerApiClient(httpClient, mockLogger.Object);
                Func<Task> func = async () => await sut.GetCustomersAsync(0, 10, null, null, null);

                //Assert
                func.Should().Throw<HttpRequestException>()
                    .WithMessage("Response status code does not indicate success: 404 (Not Found).");
            }
        }

        public class GetCustomer
        {
            [Fact]
            public async void GetCustomer_CustomerFound_ReturnCustomer()
            {
                //Arrange

                var mockLogger = new Mock<ILogger<CustomerApiClient>>();
                var customer = new TestBuilders.GetCustomer.StoreCustomerBuilder()
                    .WithTestValues()
                    .Build();

                var httpClient = new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(
                            JsonSerializer.Serialize(customer, new JsonSerializerOptions
                            {
                                Converters =
                                {
                                new JsonStringEnumConverter(),
                                new CustomerConverter<
                                        ApiClients.CustomerApi.Models.GetCustomer.Customer,
                                        ApiClients.CustomerApi.Models.GetCustomer.StoreCustomer,
                                        ApiClients.CustomerApi.Models.GetCustomer.IndividualCustomer>()
                                },
                                IgnoreReadOnlyProperties = true,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            }),
                            Encoding.UTF8,
                            "application/json"
                        )
                    };

                    return await Task.FromResult(responseMessage);
                }))
                {
                    BaseAddress = new Uri("http://baseaddress")
                };

                //Act
                var sut = new CustomerApiClient(httpClient, mockLogger.Object);
                var response = await sut.GetCustomerAsync("AW00000001");

                //Assert
                response.Should().NotBeNull();
                response.AccountNumber.Should().Be(customer.AccountNumber);
                response.Addresses.Should().BeEquivalentTo(customer.Addresses);
                response.SalesOrders.Should().BeEquivalentTo(customer.SalesOrders);

                var store = response as ApiClients.CustomerApi.Models.GetCustomer.StoreCustomer;
                store.Name.Should().Be(customer.Name);
                store.SalesPerson.Should().Be(customer.SalesPerson);
                store.Territory.Should().Be(customer.Territory);
                store.Contacts.Should().BeEquivalentTo(customer.Contacts);
            }

            [Fact]
            public void GetCustomer_CustomerNotFound_ThrowsHttpRequestException()
            {
                //Arrange

                var mockLogger = new Mock<ILogger<CustomerApiClient>>();

                var httpClient = new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                    return await Task.FromResult(responseMessage);
                }))
                {
                    BaseAddress = new Uri("http://baseaddress")
                };

                //Act
                var sut = new CustomerApiClient(httpClient, mockLogger.Object);
                Func<Task> func = async () => await sut.GetCustomerAsync("AW00000001");

                //Assert
                func.Should().Throw<HttpRequestException>()
                    .WithMessage("Response status code does not indicate success: 404 (Not Found).");
            }
        }
    }
}