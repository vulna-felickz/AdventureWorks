﻿using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace AW.Services.SharedKernel.EFCore
{
    public class AzureAdAuthenticationDbConnectionInterceptor : DbConnectionInterceptor
    {
        private static readonly string[] _azureSqlScopes = new[]
        {
            "https://database.windows.net//.default"
        };

        private static TokenCredential _credential = new ChainedTokenCredential(
                new ManagedIdentityCredential(),
                new EnvironmentCredential()
            );

        private static TokenCredential _defaultCredential = new DefaultAzureCredential();

        public AzureAdAuthenticationDbConnectionInterceptor() 
        {
        }
        public AzureAdAuthenticationDbConnectionInterceptor(
            TokenCredential credential, TokenCredential defaultCredential)
        {
            _credential = credential;
            _defaultCredential = defaultCredential;
        }

        public override InterceptionResult ConnectionOpening(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result)
        {
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedAccessToken(sqlConnection))
            {
                var tokenRequestContext = new TokenRequestContext(_azureSqlScopes);
                var token = _credential.GetToken(tokenRequestContext, default);

                sqlConnection.AccessToken = token.Token;
            }

            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedAccessToken(sqlConnection))
            {
                sqlConnection.AccessToken = await GetAzureSqlAccessToken(cancellationToken);
            }

            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }

        private static bool DoesConnectionNeedAccessToken(SqlConnection connection)
        {
            //
            // Only try to get a token from AAD if
            //  - We connect to an Azure SQL instance; and
            //  - The connection doesn't specify a username.
            //
            var connectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);

            return connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID);
        }

        private static async Task<string> GetAzureSqlAccessToken(CancellationToken cancellationToken)
        {
            // See https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-sql
            var tokenRequestContext = new TokenRequestContext(_azureSqlScopes);
            var tokenRequestResult = await _defaultCredential.GetTokenAsync(tokenRequestContext, cancellationToken);

            return tokenRequestResult.Token;
        }
    }
}
