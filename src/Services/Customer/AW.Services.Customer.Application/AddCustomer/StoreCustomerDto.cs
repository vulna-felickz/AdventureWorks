﻿using AutoMapper;
using AW.Services.Customer.Application.Common;
using System.Collections.Generic;

namespace AW.Services.Customer.Application.AddCustomer
{
    public class StoreCustomerDto : CustomerDto, IMapFrom<Domain.StoreCustomer>
    {
        public string Name { get; set; }
        public string SalesPerson { get; set; }
        public List<StoreCustomerContactDto> Contacts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.StoreCustomer, StoreCustomerDto>();
        }
    }
}