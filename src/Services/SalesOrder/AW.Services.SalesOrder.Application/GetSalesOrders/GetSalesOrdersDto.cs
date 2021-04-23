﻿using System.Collections.Generic;

namespace AW.Services.SalesOrder.Application.GetSalesOrders
{
    public class GetSalesOrdersDto
    {
        public IEnumerable<SalesOrderDto> SalesOrders { get; set; }
        public int TotalSalesOrders { get; set; }
    }
}