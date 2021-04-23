﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AW.Services.Customer.REST.API.Models.GetCustomers
{
    public abstract class Customer
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CustomerType CustomerType { get; set; }
        public string AccountNumber { get; set; }
        public string Territory { get; set; }
        public List<CustomerAddress> Addresses { get; set; }
    }
}