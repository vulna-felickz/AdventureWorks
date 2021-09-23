﻿using AW.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AW.UI.Web.Internal.ViewModels.Customer
{
    public class CustomersIndexViewModel
    {
        public List<CustomerViewModel> Customers { get; set; }
        public IEnumerable<SelectListItem> Territories { get; set; }
        public IEnumerable<SelectListItem> CustomerTypes { get; set; }
        public string TerritoryFilterApplied { get; set; }
        public CustomerType? CustomerTypeFilterApplied { get; set; }
        public string AccountNumber { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}