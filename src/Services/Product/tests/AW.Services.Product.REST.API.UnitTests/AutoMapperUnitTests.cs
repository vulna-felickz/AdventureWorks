﻿using AutoMapper;
using Xunit;

namespace AW.Services.Product.REST.API.UnitTests
{
    public class AutoMapperUnitTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.AddProfile<Core.AutoMapper.MappingProfile>();
            });
            config.AssertConfigurationIsValid();
        }
    }
}