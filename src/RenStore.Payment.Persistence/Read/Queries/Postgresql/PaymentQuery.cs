using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.Read.Base;

namespace RenStore.Payment.Persistence.Read.Queries.Postgresql;

internal sealed class PaymentQuery(
    PaymentDbContext context, 
    ILogger<PaymentQuery> logger)
    : DapperQueryBase(context, logger), 
      IPaymentQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "payment_id"          AS Id,
            "order_id"            AS OrderId,
            "customer_id"         AS CustomerId,
            "amount"              AS Amount,
            "refunded_amount"     AS RefundedAmount,
            "currency"            AS Currency,
            "status"              AS Status,
            "provider"            AS Provider,
            "payment_method"      AS PaymentMethod,
            "external_payment_id" AS ExternalPaymentId,
            "failure_reason"      AS FailureReason,
            "expires_at"          AS ExpiresAt,
            "captured_at"         AS CapturedAt,
            "created_at"          AS CreatedAt,
            "updated_at"          AS UpdatedAt
        FROM "payments"
        """;

    private static readonly Dictionary<PaymentSortBy, string> SortMapping = new()
    {
        { PaymentSortBy.CreatedAt, "created_at" },
        { PaymentSortBy.UpdatedAt, "updated_at" },
        { PaymentSortBy.Amount,    "amount" },
        { PaymentSortBy.Status,    "status" },
        { PaymentSortBy.Provider,  "provider" },
    };

    public async Task<PaymentReadModel?> FindByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        if (paymentId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(paymentId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "payment_id" = @PaymentId;
                       """;

            return await connection.QueryFirstOrDefaultAsync<PaymentReadModel>(
                new CommandDefinition(
                    sql,
                    new { PaymentId = paymentId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<PaymentReadModel?> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "order_id" = @OrderId;
                       """;

            return await connection.QueryFirstOrDefaultAsync<PaymentReadModel>(
                new CommandDefinition(
                    sql,
                    new { OrderId = orderId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<PaymentReadModel>> FindByCustomerIdAsync(
        Guid             customerId,
        PaymentSortBy    sortBy = PaymentSortBy.CreatedAt,
        uint             page = 1,
        uint             pageSize = 25,
        bool             descending = true,
        PaymentStatus?   status = null,
        CancellationToken cancellationToken = default)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(customerId));

        if (!SortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder($"""
                {BaseSql}
                WHERE "customer_id" = @CustomerId
                """);

            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", customerId);
            parameters.Add("Count",  pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (status.HasValue)
            {
                parameters.Add("Status", status.Value.ToString());
                sql.Append(""" AND "status" = @Status """);
            }

            sql.Append($"""
                        ORDER BY "{column}" {pageRequest.Direction}
                        LIMIT @Count
                        OFFSET @Offset;
                        """);

            var result = await connection.QueryAsync<PaymentReadModel>(
                new CommandDefinition(
                    commandText: sql.ToString(),
                    parameters:  parameters,
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<PaymentReadModel>> FindAllAsync(
        PaymentSortBy    sortBy = PaymentSortBy.CreatedAt,
        uint             page = 1,
        uint             pageSize = 25,
        bool             descending = true,
        PaymentStatus?   status = null,
        CancellationToken cancellationToken = default)
    {
        if (!SortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(BaseSql);

            var parameters = new DynamicParameters();
            parameters.Add("Count",  pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (status.HasValue)
            {
                parameters.Add("Status", status.Value.ToString());
                sql.Append(""" WHERE "status" = @Status """);
            }

            sql.Append($"""
                        ORDER BY "{column}" {pageRequest.Direction}
                        LIMIT @Count
                        OFFSET @Offset;
                        """);

            var result = await connection.QueryAsync<PaymentReadModel>(
                new CommandDefinition(
                    commandText: sql.ToString(),
                    parameters:  parameters,
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
}