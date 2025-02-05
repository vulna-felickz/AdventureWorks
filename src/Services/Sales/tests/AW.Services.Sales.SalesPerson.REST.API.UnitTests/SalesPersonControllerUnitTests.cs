using AutoFixture.Xunit2;
using AW.Services.Sales.Core.Handlers.GetSalesPerson;
using AW.Services.Sales.Core.Handlers.GetSalesPersons;
using AW.Services.Sales.SalesPerson.REST.API.Controllers;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AW.Services.Sales.SalesPerson.REST.API.UnitTests
{
    public class SalesPersonControllerUnitTests
    {
        [Theory, AutoMapperData(typeof(MappingProfile), typeof(Core.AutoMapper.MappingProfile))]
        public async Task GetSalesPersons_ShouldReturnSalesPersons_WhenGivenSalesPersons(
            [Frozen] Mock<IMediator> mockMediator,
            List<Core.Handlers.GetSalesPersons.SalesPersonDto> salesPersons,
            [Greedy] SalesPersonController sut,
            GetSalesPersonsQuery query
        )
        {
            //Arrange
            mockMediator.Setup(x => x.Send(
                It.IsAny<GetSalesPersonsQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(salesPersons);

            //Act
            var actionResult = await sut.GetSalesPersons(query);

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            var result = okObjectResult?.Value as List<Core.Models.SalesPerson>;
            result?.Count.Should().Be(salesPersons.Count);
        }

        [Theory, AutoMapperData(typeof(MappingProfile))]
        public async Task GetSalesPersons_ShouldReturnNotFound_WhenGivenNoSalesPersons(
            [Greedy] SalesPersonController sut,
            GetSalesPersonsQuery query
        )
        {
            //Act
            var actionResult = await sut.GetSalesPersons(query);

            //Assert
            var notFoundResult = actionResult as NotFoundResult;
            notFoundResult.Should().NotBeNull();
        }

        [Theory, AutoMapperData(typeof(MappingProfile), typeof(Core.AutoMapper.MappingProfile))]
        public async Task GetSalesPerson_ShouldReturnSalesPerson_GivenSalesPerson(
            [Frozen] Mock<IMediator> mockMediator,
            Core.Handlers.GetSalesPerson.SalesPersonDto salesPerson,
            [Greedy] SalesPersonController sut,
            GetSalesPersonQuery query
        )
        {
            //Arrange
            mockMediator.Setup(x => x.Send(
                It.IsAny<GetSalesPersonQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(salesPerson);

            //Act
            var actionResult = await sut.GetSalesPerson(query);

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            var result = okObjectResult?.Value as Core.Models.SalesPerson;
            result!.Name!.FullName.Should().Be(salesPerson.Name!.FullName);
        }

        [Theory, AutoMapperData(typeof(MappingProfile))]
        public async Task GetSalesPerson_ShouldReturnNotFound_GivenNoSalesPerson(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SalesPersonController sut,
            GetSalesPersonQuery query
        )
        {
            //Arrange
            mockMediator.Setup(x => x.Send(
                It.IsAny<GetSalesPersonQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Core.Handlers.GetSalesPerson.SalesPersonDto?)null);

            //Act
            var actionResult = await sut.GetSalesPerson(query);

            //Assert
            var notFoundResult = actionResult as NotFoundResult;
            notFoundResult.Should().NotBeNull();
        }
    }
}