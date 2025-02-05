﻿using AutoMapper;
using AW.SharedKernel.AutoMapper;
using AW.Services.Customer.Core.Handlers.GetCustomer;
using AW.SharedKernel.ValueTypes;

namespace AW.Services.Customer.Core.Models.GetCustomers
{
    public class Person : IMapFrom<PersonDto>
    {
        public string? Title { get; set; }
        public NameFactory? Name { get; set; }
        public string? Suffix { get; set; }
        public List<PersonEmailAddressDto> EmailAddresses { get; set; } = new();
        public List<PersonPhoneDto> PhoneNumbers { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonDto, Person>();
        }
    }
}