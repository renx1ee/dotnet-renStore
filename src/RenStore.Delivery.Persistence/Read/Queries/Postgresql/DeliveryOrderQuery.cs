using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class DeliveryOrderQuery(
    DeliveryDbContext           context,
    ILogger<DeliveryOrderQuery> logger)
    : DapperQueryBase(context, logger), 
      IDeliveryOrderQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "delivery_order_id"             AS Id,
            "order_id"                      AS OrderId,
            "delivery_tariff_id"            AS DeliveryTariffId,
            "status"                        AS Status,
            "current_sorting_center_id"     AS CurrentSortingCenterId,
            "destination_sorting_center_id" AS DestinationSortingCenterId,
            "pickup_point_id"               AS PickupPointId,
            "created_at"                    AS CreatedAt,
            "delivered_at"                  AS DeliveredAt,
            "deleted_at"                    AS DeletedAt
        FROM "delivery_orders"
        """;

    private static readonly Dictionary<DeliveryOrderSortBy, string> SortMapping = new()
    {
        { DeliveryOrderSortBy.CreatedAt,   "created_at" },
        { DeliveryOrderSortBy.DeliveredAt, "delivered_at" },
        { DeliveryOrderSortBy.Status,      "status" },
    };

    public async Task<DeliveryOrderReadModel?> FindByIdAsync(
        Guid              id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"{BaseSql} WHERE \"delivery_order_id\" = @Id;";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryOrderReadModel>(
                    new CommandDefinition(sql,
                        new { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<DeliveryOrderReadModel?> FindByOrderIdAsync(
        Guid              orderId,
        CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"{BaseSql} WHERE \"order_id\" = @OrderId;";

            return await connection.QueryFirstOrDefaultAsync<DeliveryOrderReadModel>(
                new CommandDefinition(sql,
                    new { OrderId = orderId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<DeliveryOrderReadModel>> FindAllAsync(
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.CreatedAt,
        uint                page = 1,
        uint                pageSize = 25,
        bool                descending = true,
        DeliveryStatus?     status = null,
        CancellationToken   cancellationToken = default)
    {
        if (!SortMapping.TryGetValue(sortBy, out var column))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql        = new StringBuilder(BaseSql);
            var parameters = new DynamicParameters();
            parameters.Add("Count",  pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (status.HasValue)
            {
                parameters.Add("Status", status.Value.ToString());
                sql.Append(""" WHERE "status" = @Status """);
            }

            sql.Append($""" ORDER BY "{column}" {pageRequest.Direction} LIMIT @Count OFFSET @Offset; """);

            var result = await connection.QueryAsync<DeliveryOrderReadModel>(
                new CommandDefinition(sql.ToString(), parameters,
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e) { throw Wrap(e); }
    }
}