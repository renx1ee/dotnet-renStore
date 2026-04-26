using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Order.Application.Enums;
using RenStore.Order.Domain.ReadModels;
using RenStore.Order.Persistence.Read.Base;

namespace RenStore.Order.Persistence.Read.Queries;

internal sealed class OrderQuery(OrderingDbContext context, ILogger<OrderQuery> logger)
    : DapperQueryBase(context, logger),
      RenStore.Order.Application.Abstractions.Queries.IOrderQuery
{
    private static readonly Dictionary<OrderSortBy, string> _sortMapping = new()
    {
        { OrderSortBy.Id,              "order_id" },
        { OrderSortBy.CustomerId,      "customer_id" },
        { OrderSortBy.Status,          "status" },
        { OrderSortBy.TotalAmount,     "total_amount" },
        { OrderSortBy.CreatedAt,       "created_at" },
        { OrderSortBy.UpdatedAt,       "updated_at" },
    };

    public async Task<OrderReadModel?> FindByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {GetBaseSql()}
                       WHERE "order_id" = @Id;
                       """;

            return await connection.QueryFirstOrDefaultAsync<OrderReadModel>(
                new CommandDefinition(
                    sql,
                    new { Id = orderId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<OrderReadModel>> FindByCustomerIdAsync(
        Guid customerId,
        OrderSortBy sortBy = OrderSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(customerId));

        if (!_sortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                {GetBaseSql()}
                WHERE "customer_id" = @CustomerId
                """);

            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", customerId);
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (onlyActive)
            {
                sql.Append(""" AND "status" = 'active' """);
            }
            else
            {
                if (isDeleted.HasValue)
                {
                    sql.Append(isDeleted.Value
                        ? """ AND "status" = 'deleted' """
                        : """ AND "status" != 'deleted' """);
                }
            }

            sql.Append($"""
                        ORDER BY "{column}" {pageRequest.Direction}
                        LIMIT @Count
                        OFFSET @Offset;
                        """);

            var result = await connection.QueryAsync<OrderReadModel>(
                new CommandDefinition(
                    commandText: sql.ToString(),
                    parameters: parameters,
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

    private static string GetBaseSql()
    {
        return """
            SELECT
                "order_id"             AS Id,
                "customer_id"          AS CustomerId,
                "status"               AS Status,
                "shipping_address"     AS ShippingAddress,
                "tracking_number"      AS TrackingNumber,
                "cancellation_reason"  AS CancellationReason,
                "total_amount"         AS TotalAmount,
                "created_at"           AS CreatedAt,
                "updated_at"           AS UpdatedAt
            FROM "orders"
            """;
    }
}