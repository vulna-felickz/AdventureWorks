﻿using AW.Services.SalesOrder.Application.Common;
using System.Reflection;

namespace AW.Services.SalesOrder.WCF
{
    public class MappingProfile : BaseMappingProfile
    {
        public MappingProfile() : base()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}