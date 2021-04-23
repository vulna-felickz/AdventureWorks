﻿using AW.UI.Web.Internal.ApiClients.CustomerApi.Models.GetCustomers;

namespace AW.UI.Web.Internal.UnitTests.TestBuilders.GetCustomers
{
    public class PersonBuilder
    {
        private Person person = new Person();

        public PersonBuilder Title(string title)
        {
            person.Title = title;
            return this;
        }

        public PersonBuilder FirstName(string firstName)
        {
            person.FirstName = firstName;
            return this;
        }

        public PersonBuilder MiddleName(string middleName)
        {
            person.MiddleName = middleName;
            return this;
        }

        public PersonBuilder LastName(string lastName)
        {
            person.LastName = lastName;
            return this;
        }

        public PersonBuilder Suffix(string suffix)
        {
            person.Suffix = suffix;
            return this;
        }

        public PersonBuilder WithTestValues()
        {
            person = new Person
            {
                FirstName = "Orlando",
                MiddleName = "N.",
                LastName = "Gee"
            };

            return this;
        }

        public Person Build()
        {
            return person;
        }
    }
}