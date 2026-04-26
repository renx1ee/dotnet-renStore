using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Order.Application.Enums;
using RenStore.Order.Domain.ReadModels;
using RenStore.Order.Persistence.Read.Base;

namespace RenStore.Order.Persistence.Read.Queries;

internal sealed class OrderItemQuery(OrderingDbContext context, ILogger<OrderItemQuery> logger) 
    : DapperQueryBase(context, logger),
      RenStore.Order.Application.Abstractions.Queries.IOrderItemQuery
{
    private static readonly Dictionary<OrderItemSortBy, string> _sortMapping = new()
    {
        { OrderItemSortBy.Id,        "order_item_id" },
        { OrderItemSortBy.OrderId,   "order_id" },
        { OrderItemSortBy.VariantId, "variant_id" },
        { OrderItemSortBy.SizeId,    "size_id" },
        { OrderItemSortBy.Quantity,  "quantity" },
        { OrderItemSortBy.Price,     "price_amount" },
        { OrderItemSortBy.Status,    "status" },
        { OrderItemSortBy.CreatedAt, "created_at" },
        { OrderItemSortBy.UpdatedAt, "updated_at" },
    };
    
    public async Task<OrderItemReadModel?> FindByIdAsync(
        Guid orderItemId,
        CancellationToken cancellationToken)
    {
        if (orderItemId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderItemId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {GetBaseSql()}
                       WHERE "order_item_id" = @Id;
                       """;

            return await connection.QueryFirstOrDefaultAsync<OrderItemReadModel>(
                new CommandDefinition(
                    sql,
                    new { Id = orderItemId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<OrderItemReadModel>> FindByOrderIdAsync(
        Guid orderId,
        OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));

        if (!_sortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                {GetBaseSql()}
                WHERE "order_id" = @OrderId
                """);
            
            var parameters = new DynamicParameters();
            parameters.Add("OrderId", orderId);
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

            var result = await connection
                .QueryAsync<OrderItemReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters,
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

    public async Task<IReadOnlyList<OrderItemReadModel>> FindByVariantIdAsync(
        Guid variantId,
        OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));

        if (!_sortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            var parameters = new DynamicParameters();
            parameters.Add("VariantId", variantId);
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            var sql = new StringBuilder(
                $"""
                {GetBaseSql()}
                WHERE "variant_id" = @VariantId
                """);
            
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

            var result = await connection.QueryAsync<OrderItemReadModel>(
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

    private string GetBaseSql()
    {
        return """
            SELECT
                "order_item_id"          AS OrderItemId,
                "order_id"               AS OrderId,
                "variant_id"             AS VariantId,
                "size_id"                AS SizeId,
                "quantity"               AS Quantity,
                "price_amount"           AS PriceAmount,
                "currency"               AS Currency,
                "product_name_snapshot"  AS ProductNameSnapshot,
                "status"                 AS Status,
                "cancellation_reason"    AS CancellationReason,
                "created_at"             AS CreatedAt,
                "updated_at"             AS UpdatedAt
            FROM "order_items"
            """;
    }
}