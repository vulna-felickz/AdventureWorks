﻿using AW.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AW.Services.SalesOrder.Core.Entities
{
    public class SalesOrder : IAggregateRoot
    {
        public int Id { get; set; }
        public byte RevisionNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public SalesOrderStatus Status { get; set; }

        public bool OnlineOrderFlag { get; set; }

        public string SalesOrderNumber { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }

        public string SalesPerson { get; set; }

        public string Territory { get; set; }

        public Address BillToAddress { get; set; }

        public Address ShipToAddress { get; set; }

        public string ShipMethod { get; set; }

        public CreditCard CreditCard { get; set; }

        public decimal SubTotal
        {
            get
            {
                return OrderLines.Select(x => x.LineTotal).Sum();
            }
        }

        public decimal TaxAmt { get; set; }

        public decimal Freight { get; set; }

        public decimal TotalDue
        {
            get
            {
                return SubTotal + TaxAmt + Freight;
            }
        }

        public string Comment { get; set; }

        public List<SalesOrderLine> OrderLines { get; set; } = new List<SalesOrderLine>();

        public List<SalesOrderSalesReason> SalesReasons { get; set; } = new List<SalesOrderSalesReason>();
    }
}