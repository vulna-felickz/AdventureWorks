﻿using AW.Services.Sales.Core.AutoMapper;
using AW.Services.Sales.Core.Handlers.GetSalesOrders;
using AW.SharedKernel.UnitTesting;
using FluentValidation.TestHelper;
using Xunit;

namespace AW.Services.Sales.Core.UnitTests.Handlers
{
    public class GetSalesOrdersQueryValidatorUnitTests
    {
        [Theory, AutoMapperData(typeof(MappingProfile))]
        public void TestValidate_WithValidPageSize_NoValidationError(
            GetSalesOrdersQueryValidator sut,
            GetSalesOrdersQuery query
        )
        {
            //Act
            var result = sut.TestValidate(query);

            //Assert
            result.ShouldNotHaveValidationErrorFor(query => query.PageSize);
            result.ShouldNotHaveValidationErrorFor(query => query.PageIndex);
        }

        [Theory, AutoMapperData(typeof(MappingProfile))]
        public void TestValidate_WithInvalidPageSize_ValidationError(
            GetSalesOrdersQueryValidator sut
        )
        {
            //Arrange
            var query = new GetSalesOrdersQuery(0, 0, string.Empty);

            //Act
            var result = sut.TestValidate(query);

            //Assert
            result.ShouldHaveValidationErrorFor(query => query.PageSize)
                .WithErrorMessage("Page size must be positive");
        }

        [Theory, AutoMapperData(typeof(MappingProfile))]
        public void TestValidate_WithInvalidPageIndex_ValidationError(
            GetSalesOrdersQueryValidator sut
        )
        {
            //Arrange
            var query = new GetSalesOrdersQuery(-1, 0, string.Empty);

            //Act
            var result = sut.TestValidate(query);

            //Assert
            result.ShouldHaveValidationErrorFor(query => query.PageIndex)
                .WithErrorMessage("Page index must be 0 or positive");
        }
    }
}