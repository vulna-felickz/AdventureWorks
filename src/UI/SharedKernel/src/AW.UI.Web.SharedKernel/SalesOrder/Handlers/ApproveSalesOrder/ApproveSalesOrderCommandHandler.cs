﻿using Ardalis.GuardClauses;
using AW.SharedKernel.Extensions;
using AW.UI.Web.SharedKernel.Interfaces.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AW.UI.Web.SharedKernel.SalesOrder.Handlers.ApproveSalesOrder
{
    public class ApproveSalesOrderCommandHandler : IRequestHandler<ApproveSalesOrderCommand>
    {
        private readonly ILogger<ApproveSalesOrderCommandHandler> _logger;
        private readonly ISalesOrderApiClient _client;

        public ApproveSalesOrderCommandHandler(ILogger<ApproveSalesOrderCommandHandler> logger, ISalesOrderApiClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<Unit> Handle(ApproveSalesOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Approving sales order {SalesOrderNumber}", request.SalesOrderNumber);
            Guard.Against.NullOrEmpty(request.SalesOrderNumber, _logger);

            await _client.ApproveSalesOrderAsync(request.SalesOrderNumber!);
            _logger.LogInformation("Approved sales order {SalesOrderNumber}", request.SalesOrderNumber);

            return Unit.Value;
        }
    }
}