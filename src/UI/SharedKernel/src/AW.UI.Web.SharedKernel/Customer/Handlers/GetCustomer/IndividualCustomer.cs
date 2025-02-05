﻿namespace AW.UI.Web.SharedKernel.Customer.Handlers.GetCustomer
{
    public class IndividualCustomer : Customer
    {
        public override string? CustomerName => Person?.Name?.FullName;
        public Person? Person { get; set; }
    }
}