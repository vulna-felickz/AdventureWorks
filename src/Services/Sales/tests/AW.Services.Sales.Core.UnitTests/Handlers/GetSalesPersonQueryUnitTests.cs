using AutoFixture.Xunit2;
using AW.Services.Sales.Core.AutoMapper;
using AW.Services.Sales.Core.Handlers.GetSalesPerson;
using AW.Services.Sales.Core.Specifications;
using AW.Services.SharedKernel.Interfaces;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using Moq;
using Xunit;

namespace AW.Services.Sales.Core.UnitTests
{
    public class GetSalesPersonQueryUnitTests
    {
        [Theory, AutoMapperData(typeof(MappingProfile))]
        public async Task Handle_SalesPersonExists_ReturnSalesPerson(
            Core.Entities.SalesPerson salesPerson,
            [Frozen] Mock<IRepository<Core.Entities.SalesPerson>> salesPersonRepoMock,
            GetSalesPersonQueryHandler sut,
            GetSalesPersonQuery query
        )
        {
            //Arrange            
            salesPersonRepoMock.Setup(x => x.SingleOrDefaultAsync(
                It.IsAny<GetSalesPersonSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(salesPerson);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            salesPersonRepoMock.Verify(x => x.SingleOrDefaultAsync(
                It.IsAny<GetSalesPersonSpecification>(),
                It.IsAny<CancellationToken>()
            ));

            result!.Name!.FullName.Should().Be(salesPerson!.Name!.FullName);
        }

        [Theory, AutoMapperData(typeof(MappingProfile))]
        public void Handle_SalesPersonsNull_ThrowsArgumentNullException(
            [Frozen] Mock<IRepository<Core.Entities.SalesPerson>> salesPersonRepoMock,
            GetSalesPersonQueryHandler sut,
            GetSalesPersonQuery query
        )
        {
            //Arrange
            salesPersonRepoMock.Setup(x => x.SingleOrDefaultAsync(
                It.IsAny<GetSalesPersonSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Core.Entities.SalesPerson?)null);

            //Act
            Func<Task> func = async () => await sut.Handle(query, CancellationToken.None);

            //Assert
            func.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'salesPerson')");
        }
    }
}