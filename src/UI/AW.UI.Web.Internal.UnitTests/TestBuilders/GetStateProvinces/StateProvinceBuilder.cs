﻿using AW.UI.Web.Internal.ApiClients.ReferenceDataApi.Models.GetStateProvinces;

namespace AW.UI.Web.Internal.UnitTests.TestBuilders.GetStateProvinces
{
    public class StateProvinceBuilder
    {
        private StateProvince stateProvince = new StateProvince();

        public StateProvinceBuilder Name(string name)
        {
            stateProvince.Name = name;
            return this;
        }

        public StateProvince Build()
        {
            return stateProvince;
        }
    }
}