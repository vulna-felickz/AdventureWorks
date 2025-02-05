﻿using Ardalis.Specification;

namespace AW.Services.Sales.Core.Specifications
{
    public class GetSalesOrderSpecification : Specification<Entities.SalesOrder>, ISingleResultSpecification<Entities.SalesOrder>
    {
        public GetSalesOrderSpecification(string salesOrderNumber) : base()
        {
            Query
                .Include(s => s.Customer)
                .Include("Customer.Person")
                .Include(s => s.OrderLines)
                .Include(s => s.SalesReasons)
                .Where(c => c.SalesOrderNumber == salesOrderNumber);
        }
    }
}