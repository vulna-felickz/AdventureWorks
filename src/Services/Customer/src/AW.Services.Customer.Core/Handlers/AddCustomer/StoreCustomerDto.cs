﻿using AutoMapper;
using AW.SharedKernel.AutoMapper;

namespace AW.Services.Customer.Core.Handlers.AddCustomer
{
    public class StoreCustomerDto : CustomerDto, IMapFrom<Entities.StoreCustomer>
    {
        public string? Name { get; set; }
        public string? SalesPerson { get; set; }
        public List<StoreCustomerContactDto> Contacts { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StoreCustomerDto, Entities.StoreCustomer>()
                .ForMember(_ => _.Id, opt => opt.Ignore())
                .ForMember(_ => _.Addresses, opt => 
                    opt.MapFrom((src, dest, member, ctx) =>
                        {
                            src.Addresses.ForEach(customerAddress =>
                                dest.AddAddress(
                                    ctx.Mapper.Map<Entities.CustomerAddress>(customerAddress)
                                )
                            );

                            return dest.Addresses;
                        }                        
                    )
                )
                .ForMember(_ => _.Contacts, opt =>
                    opt.MapFrom((src, dest, member, ctx) =>
                        {
                            src.Contacts.ForEach(customerContact =>
                                dest.AddContact(
                                    ctx.Mapper.Map<Entities.StoreCustomerContact>(customerContact)
                                )
                            );

                            return dest.Contacts;
                        }
                    )
                )
                .ForMember(_ => _.SalesOrders, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}