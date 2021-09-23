﻿using AutoMapper;
using AW.SharedKernel.AutoMapper;

namespace AW.Services.Customer.Core.Handlers.AddCustomerAddress
{
    public class CustomerAddressDto : IMapFrom<Entities.CustomerAddress>
    {
        public AddressDto Address { get; set; }
        public string AddressType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CustomerAddressDto, Entities.CustomerAddress>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.AddressID, opt => opt.Ignore());
        }
    }
}