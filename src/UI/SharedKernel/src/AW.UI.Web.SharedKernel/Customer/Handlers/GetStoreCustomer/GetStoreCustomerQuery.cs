﻿using MediatR;

namespace AW.UI.Web.SharedKernel.Customer.Handlers.GetStoreCustomer
{
    public class GetStoreCustomerQuery : IRequest<StoreCustomer>
    {
        public GetStoreCustomerQuery(string? accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string? AccountNumber { get; set; }
    }
}