﻿using MediatR;
using System.Collections.Generic;

namespace AW.Services.ReferenceData.Application.StateProvince.GetStatesProvinces
{
    public class GetStatesProvincesQuery : IRequest<List<StateProvince>>
    {
        public string CountryRegionCode { get; set; }
    }
}