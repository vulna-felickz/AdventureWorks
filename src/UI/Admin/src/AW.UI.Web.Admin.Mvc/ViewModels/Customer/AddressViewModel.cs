﻿using AutoMapper;
using AW.SharedKernel.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace AW.UI.Web.Admin.Mvc.ViewModels.Customer
{
    public class AddressViewModel : IMapFrom<SharedKernel.Customer.Handlers.GetCustomers.Address>
    {
        [Display(Name = "Address line 1")]
        [Required]
        public string? AddressLine1 { get; set; }
        [Display(Name = "Address line 2")]
        public string? AddressLine2 { get; set; }
        [Display(Name = "Postal code")]
        [Required]
        public string? PostalCode { get; set; }
        [Display(Name = "City")]
        [Required]
        public string? City { get; set; }
        [Display(Name = "State/province")]
        public string? StateProvinceCode { get; set; }
        [Display(Name = "Country")]
        public string? CountryRegionCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SharedKernel.Customer.Handlers.GetCustomers.Address, AddressViewModel>();
            profile.CreateMap<SharedKernel.Customer.Handlers.GetCustomer.Address, AddressViewModel>()
                .ReverseMap();
            profile.CreateMap<SharedKernel.Customer.Handlers.GetIndividualCustomer.Address, AddressViewModel>()
                .ReverseMap();
            profile.CreateMap<SharedKernel.Customer.Handlers.GetStoreCustomer.Address, AddressViewModel>();

            profile.CreateMap<AddressViewModel, SharedKernel.Customer.Handlers.UpdateCustomer.Address>();
        }
    }
}