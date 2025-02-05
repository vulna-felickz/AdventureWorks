﻿using Ardalis.GuardClauses;
using AutoMapper;
using AW.Services.Sales.Core.Guards;
using AW.Services.Sales.Core.Specifications;
using AW.Services.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AW.Services.Sales.Core.Handlers.GetSalesOrders
{
    public class GetSalesOrdersQueryHandler : IRequestHandler<GetSalesOrdersQuery, GetSalesOrdersDto>
    {
        private readonly ILogger<GetSalesOrdersQueryHandler> _logger;
        private readonly IRepository<Entities.SalesOrder> _repository;
        private readonly IMapper _mapper;

        public GetSalesOrdersQueryHandler(
            ILogger<GetSalesOrdersQueryHandler> logger,
            IRepository<Entities.SalesOrder> repository,
            IMapper mapper) =>
                (_logger, _repository, _mapper) = (logger, repository, mapper);

        public async Task<GetSalesOrdersDto> Handle(GetSalesOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handle called");

            _logger.LogInformation("Getting sales orders from database");
            var spec = new GetSalesOrdersPaginatedSpecification(
                request.PageIndex,
                request.PageSize,
                request.Territory
            );
            var countSpec = new CountSalesOrdersSpecification(
                request.Territory
            );

            var orders = await _repository.ListAsync(spec, cancellationToken);
            Guard.Against.SalesOrdersNull(orders, _logger);

            _logger.LogInformation("Returning sales orders");
            return new GetSalesOrdersDto
            {
                SalesOrders = _mapper.Map<List<SalesOrderDto>>(orders),
                TotalSalesOrders = await _repository.CountAsync(countSpec, cancellationToken)
            };
        }
    }
}