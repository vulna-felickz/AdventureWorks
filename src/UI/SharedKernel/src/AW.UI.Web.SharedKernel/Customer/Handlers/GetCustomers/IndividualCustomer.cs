﻿namespace AW.UI.Web.SharedKernel.Customer.Handlers.GetCustomers
{
    public class IndividualCustomer : Customer
    {
        public override string? CustomerName => Person?.Name?.FullName;
        public Person? Person { get; set; }
    }
}