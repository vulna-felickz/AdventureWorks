﻿using AW.Services.Customer.Application.Common;
using AW.Services.Customer.Application.GetCustomers;

namespace AW.Services.Customer.REST.API.Models.GetCustomers
{
    public class Address : IMapFrom<AddressDto>
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string CountryRegionCode { get; set; }
    }
}