﻿using AW.UI.Web.Infrastructure.ApiClients.SalesOrderApi.Models;
using AW.UI.Web.Internal.Controllers;
using AW.UI.Web.Internal.Interfaces;
using AW.UI.Web.Internal.Services;
using AW.UI.Web.Internal.ViewModels.SalesOrder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AW.UI.Web.Internal.UnitTests.Controllers
{
    public class SalesOrderControllerUnitTests
    {
        public class Index
        {
            [Fact]
            public async Task Index_ReturnsViewModel()
            {
                //Arrange
                var mockSalesOrderViewModelService = new Mock<ISalesOrderService>();
                mockSalesOrderViewModelService.Setup(x => x.GetSalesOrders(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CustomerType?>()
                ))
                .ReturnsAsync(new SalesOrderIndexViewModel());

                var mockSalesPersonViewModelService = new Mock<ISalesPersonViewModelService>();
                var mockReferenceDataService = new Mock<IReferenceDataService>();

                var controller = new SalesOrderController(
                    mockSalesOrderViewModelService.Object,
                    mockSalesPersonViewModelService.Object,
                    mockReferenceDataService.Object
                );

                //Act
                var actionResult = await controller.Index(
                    0, null, null
                );

                //Assert
                var viewResult = actionResult.Should().BeAssignableTo<ViewResult>().Subject;
                viewResult.Model.Should().BeAssignableTo<SalesOrderIndexViewModel>();

                mockSalesOrderViewModelService.Verify(x => x.GetSalesOrders(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CustomerType?>()
                ));
            }
        }

        public class Detail
        {
            [Fact]
            public async Task Detail_ReturnsViewModel()
            {
                //Arrange
                var mockSalesOrderViewModelService = new Mock<ISalesOrderService>();
                mockSalesOrderViewModelService.Setup(x => x.GetSalesOrder(
                    It.IsAny<string>()
                ))
                .ReturnsAsync(new SalesOrderDetailViewModel()
                );

                var mockSalesPersonViewModelService = new Mock<ISalesPersonViewModelService>();
                var mockReferenceDataService = new Mock<IReferenceDataService>();

                var controller = new SalesOrderController(
                    mockSalesOrderViewModelService.Object,
                    mockSalesPersonViewModelService.Object,
                    mockReferenceDataService.Object
                );

                //Act
                var actionResult = await controller.Detail("SO43659");

                //Assert
                var viewResult = actionResult.Should().BeAssignableTo<ViewResult>().Subject;
                var viewModel = viewResult.Model.Should().BeAssignableTo<SalesOrderDetailViewModel>().Subject;

                mockSalesOrderViewModelService.Verify(x => x.GetSalesOrder(
                    It.IsAny<string>()
                ));
            }
        }
    }
}