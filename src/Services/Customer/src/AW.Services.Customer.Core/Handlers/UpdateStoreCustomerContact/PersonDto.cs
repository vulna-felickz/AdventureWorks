﻿using AutoMapper;
using AW.SharedKernel.AutoMapper;
using AW.SharedKernel.ValueTypes;

namespace AW.Services.Customer.Core.Handlers.UpdateStoreCustomerContact
{
    public class PersonDto : IMapFrom<Entities.Person>
    {
        public string? Title { get; set; }
        public NameFactory? Name { get; set; }
        public string? Suffix { get; set; }
        public List<EmailAddressDto> EmailAddresses { get; set; } = new List<EmailAddressDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Entities.Person, PersonDto>()
                .ReverseMap();
        }
    }
}