﻿using AW.Services.Product.Domain;
using AW.Services.Product.Persistence.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AW.Services.Product.Persistence.EFCore.UnitTests
{
    public class AWContextUnitTests
    {
        [Fact]
        public void CreateDatabase_ModelConfigurationsAreApplied()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<AWContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
                
            var context = new AWContext(contextOptions);

            //Act
            context.Database.EnsureCreated();

            //Assert
            var entityTypes = context.Model.GetEntityTypes().ToList();
            entityTypes[0].Name.Should().Be(typeof(BillOfMaterials).FullName);
            entityTypes[1].Name.Should().Be(typeof(Culture).FullName);
            entityTypes[2].Name.Should().Be(typeof(Document).FullName);
            entityTypes[3].Name.Should().Be(typeof(Illustration).FullName);
            entityTypes[4].Name.Should().Be(typeof(Location).FullName);
            entityTypes[5].Name.Should().Be(typeof(Domain.Product).FullName);
            entityTypes[6].Name.Should().Be(typeof(ProductCategory).FullName);            
            entityTypes[7].Name.Should().Be(typeof(ProductCostHistory).FullName);
            entityTypes[8].Name.Should().Be(typeof(ProductDescription).FullName);
            entityTypes[9].Name.Should().Be(typeof(ProductDocument).FullName);
            entityTypes[10].Name.Should().Be(typeof(ProductInventory).FullName);
            entityTypes[11].Name.Should().Be(typeof(ProductListPriceHistory).FullName);
            entityTypes[12].Name.Should().Be(typeof(ProductModel).FullName);
            entityTypes[13].Name.Should().Be(typeof(ProductModelIllustration).FullName);
            entityTypes[14].Name.Should().Be(typeof(ProductModelProductDescriptionCulture).FullName);
            entityTypes[15].Name.Should().Be(typeof(ProductPhoto).FullName);
            entityTypes[16].Name.Should().Be(typeof(ProductProductPhoto).FullName);
            entityTypes[17].Name.Should().Be(typeof(ProductSubcategory).FullName);
            entityTypes[18].Name.Should().Be(typeof(UnitMeasure).FullName);
        }
    }
}