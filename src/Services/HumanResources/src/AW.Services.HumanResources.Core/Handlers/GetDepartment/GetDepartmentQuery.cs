﻿using MediatR;

namespace AW.Services.HumanResources.Core.Handlers.GetDepartment
{
    public class GetDepartmentQuery : IRequest<Department>
    {
        public GetDepartmentQuery(string name)
        {
            Name = name;
        }

        public string Name { get; init; }
    }
}