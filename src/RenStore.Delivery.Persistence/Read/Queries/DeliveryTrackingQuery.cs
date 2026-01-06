using System.Data;
using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class DeliveryTrackingQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.IDeliveryTrackingQuery
{
    private const string BaseSqlQuery =
        """
            SELECT 
                ""delivery_tracking_history_id"" AS Id,
                ""current_location""             AS CurrentLocation,
                ""status""                       AS Status,
                ""notes""                        AS Notes,
                ""created_date""                 AS OccuredAt,
                ""deleted_date""                 AS DeletedAt,
                ""is_deleted""                   AS IsDeleted,
                ""sorting_center_id""            AS SortingCenterId,
                ""delivery_order_id""            AS DeliveryOrderId
            FROM
                ""delivery_tracking_history""
        """;
    
    private readonly Dictionary<DeliveryTrackingSortBy, string> _sortColumnMapping = new()
    {
        { DeliveryTrackingSortBy.Id, "delivery_tracking_history_id"},
        { DeliveryTrackingSortBy.CurrentLocation, "current_location"},
        { DeliveryTrackingSortBy.Status, "status"},
        { DeliveryTrackingSortBy.CreatedAt, "created_date"},
        { DeliveryTrackingSortBy.DeletedAt, "deleted_date"}
    };
    
    public DeliveryTrackingQuery(
        ILogger<DeliveryTrackingQuery> logger,
        DeliveryDbContext context) 
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<DeliveryTrackingReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append($" WHERE \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<DeliveryTrackingReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<DeliveryTrackingReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE ""delivery_tracking_history_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryTrackingReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<DeliveryTrackingReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id: id, cancellationToken: cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryTrackingReadModel), id);
    }
    
    public async Task<IReadOnlyList<DeliveryTrackingReadModel>> FindByDeliveryOrderAsync(
        Guid deliveryOrderId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (deliveryOrderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(deliveryOrderId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""delivery_order_id"" = @Id
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                          LIMIT @Count
                          OFFSET @Offset;");

            var result = await connection
                .QueryAsync<DeliveryTrackingReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Id = deliveryOrderId,
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<DeliveryTrackingReadModel>> GetByDeliveryOrderAsync(
        Guid deliveryOrderId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByDeliveryOrderAsync(
            deliveryOrderId: deliveryOrderId,
            cancellationToken: cancellationToken,
            page: page,
            pageSize: pageSize,
            sortBy: sortBy,
            descending: descending,
            isDeleted: isDeleted);

        if (result.Count == 0)
            throw new NotFoundException(typeof(DeliveryTrackingReadModel), deliveryOrderId);

        return result;
    }
}