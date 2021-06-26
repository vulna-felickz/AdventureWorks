﻿using AW.UI.Web.Common.ApiClients.SalesOrderApi.Models;

namespace AW.UI.Web.Common.UnitTests.TestBuilders.GetSalesOrders
{
    public class AddressBuilder
    {
        private Address address = new Address();

        public AddressBuilder AddressLine1(string addressLine1)
        {
            address.AddressLine1 = addressLine1;
            return this;
        }

        public AddressBuilder AddressLine2(string addressLine2)
        {
            address.AddressLine1 = addressLine2;
            return this;
        }

        public AddressBuilder PostalCode(string postalCode)
        {
            address.PostalCode = postalCode;
            return this;
        }

        public AddressBuilder City(string city)
        {
            address.City = city;
            return this;
        }

        public AddressBuilder StateProvinceCode(string stateProvinceCode)
        {
            address.StateProvinceCode = stateProvinceCode;
            return this;
        }

        public AddressBuilder CountryRegionCode(string countryRegionCode)
        {
            address.CountryRegionCode = countryRegionCode;
            return this;
        }

        public AddressBuilder WithTestValues()
        {
            address = new Address
            {
                AddressLine1 = "42525 Austell Road",
                PostalCode = "30106",
                City = "Austell",
                StateProvinceCode = "GA",
                CountryRegionCode = "US"
            };

            return this;
        }

        public Address Build()
        {
            return address;
        }
    }
}