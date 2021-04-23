﻿using AW.Services.Customer.Application.Common;
using AW.Services.Customer.Application.GetCustomer;

namespace AW.Services.Customer.REST.API.Models.GetCustomer
{
    public class IndividualCustomer : Customer, IMapFrom<IndividualCustomerDto>
    {
        public Person Person { get; set; }
    }
}