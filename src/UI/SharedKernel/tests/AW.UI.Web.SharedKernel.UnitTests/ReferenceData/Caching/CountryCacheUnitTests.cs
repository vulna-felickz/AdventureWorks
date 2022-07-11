﻿using AutoFixture.Xunit2;
using AW.UI.Web.SharedKernel.Interfaces.Api;
using AW.UI.Web.SharedKernel.ReferenceData.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AW.UI.Web.SharedKernel.UnitTests.ReferenceData.Caching
{
    public class CountryCacheUnitTests
    {
        public class Initialize
        {
            [Theory, AutoMoqData]
            public async Task Initialize_CacheIsSet(
                [Frozen] Mock<IMemoryCache> cacheMock,
                CountryCache sut
            )
            {
                //Arrange

                //Act
                await sut.Initialize();

                //Assert
                cacheMock.Verify(_ => _.CreateEntry(
                        It.IsAny<string>()
                    )
                );
            }
        }

        public class GetData
        {
            [Theory, AutoMoqData]
            public async Task GetData_CacheNotSet_CountriesAddedToCache(
                [Frozen] Mock<IMemoryCache> cacheMock,
                [Frozen] Mock<IReferenceDataApiClient> mockClient,
                CountryCache sut,
                List<SharedKernel.ReferenceData.Handlers.GetCountries.CountryRegion> countries
            )
            {
                //Arrange
                mockClient.Setup(_ => _.GetCountriesAsync())
                    .ReturnsAsync(countries);

                //Act
                var result = await sut.GetData();

                //Assert
                result.Should().BeEquivalentTo(countries);
                cacheMock.Verify(_ => _.CreateEntry(
                        It.IsAny<string>()
                    )
                );
            }

            [Theory, AutoMoqData]
            public async Task GetData_CountriesAreCached_CacheIsNotSet(
                [Frozen] Mock<IMemoryCache> cacheMock,
                CountryCache sut,
                List<SharedKernel.ReferenceData.Handlers.GetCountries.CountryRegion> countries
            )
            {
                //Arrange
                object value = countries;
                cacheMock.Setup(_ => _.TryGetValue(
                        It.IsAny<object>(),
                        out value
                    )
                )
                .Returns(true);

                //Act
                var result = await sut.GetData();

                //Assert
                result.Should().BeEquivalentTo(countries);
                cacheMock.Verify(_ => _.CreateEntry(
                        It.IsAny<string>()
                    ),
                    Times.Never
                );
            }
        }

        public class GetDataWithPredicate
        {
            [Theory, AutoMoqData]
            public async Task GetData_FilteredCountries(
                [Frozen] Mock<IReferenceDataApiClient> mockClient,
                CountryCache sut,
                List<SharedKernel.ReferenceData.Handlers.GetCountries.CountryRegion> countries
            )
            {
                //Arrange
                mockClient.Setup(_ => _.GetCountriesAsync())
                    .ReturnsAsync(countries);

                //Act
                var result = await sut.GetData(_ => _.Name == countries[0].Name);

                //Assert
                result.Should().BeEquivalentTo(new[] { countries[0] });
            }
        }
    }
}