﻿using AutoMapper;
using AutoMapper.EquivalencyExpression;
using AW.Services.Customer.Application.Common;

namespace AW.Services.Customer.Application.UpdateCustomer
{
    public class StoreCustomerContactDto : IMapFrom<Domain.StoreCustomerContact>
    {
        public string ContactType { get; set; }
        public PersonDto ContactPerson { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.StoreCustomerContact, StoreCustomerContactDto>()
                .ReverseMap()
                .EqualityComparison((src, dest) => src.ContactType == dest.ContactType);
        }
    }
}