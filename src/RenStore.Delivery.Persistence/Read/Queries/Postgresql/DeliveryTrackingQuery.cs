using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class DeliveryTrackingQuery(
    DeliveryDbContext              context,
    ILogger<DeliveryTrackingQuery> logger)
    : DapperQueryBase(context, logger), 
      IDeliveryTrackingQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "tracking_id"        AS Id,
            "delivery_order_id"  AS DeliveryOrderId,
            "status"             AS Status,
            "current_location"   AS CurrentLocation,
            "notes"              AS Notes,
            "sorting_center_id"  AS SortingCenterId,
            "pickup_point_id"    AS PickupPointId,
            "occurred_at"        AS OccurredAt
        FROM "delivery_trackings"
        """;

    public async Task<IReadOnlyList<DeliveryTrackingReadModel>> FindByDeliveryOrderIdAsync(
        Guid              deliveryOrderId,
        CancellationToken cancellationToken)
    {
        if (deliveryOrderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(deliveryOrderId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "delivery_order_id" = @DeliveryOrderId
                       ORDER BY "occurred_at" ASC;
                       """;

            var result = await connection.QueryAsync<DeliveryTrackingReadModel>(
                new CommandDefinition(sql,
                    new { DeliveryOrderId = deliveryOrderId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e) { throw Wrap(e); }
    }
}