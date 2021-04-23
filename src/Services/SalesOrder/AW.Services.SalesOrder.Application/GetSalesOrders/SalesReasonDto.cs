﻿using AutoMapper;
using AW.Services.SalesOrder.Application.Common;

namespace AW.Services.SalesOrder.Application.GetSalesOrders
{
    public class SalesReasonDto : IMapFrom<Domain.SalesReason>
    {
        public string Name { get; set; }
        public string ReasonType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.SalesReason, SalesReasonDto>();
        }
    }
}