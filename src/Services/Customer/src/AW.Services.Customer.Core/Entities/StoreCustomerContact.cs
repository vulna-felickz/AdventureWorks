﻿namespace AW.Services.Customer.Core.Entities
{
    public class StoreCustomerContact
    {
        public int Id { get; set; }
        public string ContactType { get; set; }

        public int StoreCustomerId { get; set; }

        public Person ContactPerson { get; set; }
        public int ContactPersonId { get; set; }
    }
}