﻿using AutoMapper;
using AW.Services.Customer.Application.AddCustomerAddress;
using AW.Services.Customer.Application.Common;

namespace AW.Services.Customer.WCF.Messages.AddCustomerAddress
{
    public class Address : IMapFrom<AddressDto>
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string CountryRegionCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Address, AddressDto>();
        }
    }
}