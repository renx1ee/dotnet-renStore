using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Inventory.Application.ReadModels;
using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.Persistence.Read.Queries;

internal sealed class ReservationQuery(InventoryDbContext context, ILogger<ReservationQuery> logger) 
    : RenStore.Inventory.Persistence.Read.Base.DapperQueryBase(context, logger),
      RenStore.Inventory.Application.Abstractions.Queries.IReservationQuery
{
    private static readonly Dictionary<ReservationSortBy, string> _reservationSortColumnMapping = new()
    {
        { ReservationSortBy.Id,         "id" },
        { ReservationSortBy.Quantity,   "quantity" },
        { ReservationSortBy.Status,     "status" },
        { ReservationSortBy.CreatedAt,  "created_date" },
        { ReservationSortBy.UpdatedAt,  "updated_date" },
        { ReservationSortBy.ExpiresAt,  "expires_date" },
        { ReservationSortBy.DeletedAt,  "deleted_date" },
        { ReservationSortBy.VariantId,  "variant_id" },
        { ReservationSortBy.SizeId,     "size_id" },
        { ReservationSortBy.OrderId,    "order_id" },
    };
    
    public async Task<VariantReservationDto?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = 
                $"""
                {GetBaseSql()}
                WHERE "id" = @Id;
                """;

            return await connection
                .QueryFirstOrDefaultAsync<VariantReservationDto>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<VariantReservationDto>> FindByVariantIdAsync(
        Guid variantId,
        ReservationSortBy sortBy = ReservationSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? includeExpired = null,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));

        if (!_reservationSortColumnMapping.TryGetValue(sortBy, out var columnMapping))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(
            page: page,
            pageSize: pageSize,
            descending: descending);
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                 {GetBaseSql()}
                 WHERE "variant_id" = @VariantId
                 """);

            var parameters = new DynamicParameters();
            parameters.Add("VariantId", variantId);
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);
            
            if (includeExpired.HasValue)
            {
                if (includeExpired.Value)
                {
                    sql.Append(""" AND ("expires_date" >= NOW() OR "expires_date" IS NULL) """);
                }
                else
                {
                    sql.Append(""" AND "expires_date" < NOW() """);
                }
            }
            
            if (isDeleted.HasValue)
            {
                sql.Append(""" AND "deleted_date" IS NOT NULL """);  // для мягкого удаления
                // или если есть флаг is_deleted:
                // sql.Append(""" AND "is_deleted" = @IsDeleted """);
                // parameters.Add("IsDeleted", isDeleted.Value);
            }
            else
            {
                sql.Append(""" AND "deleted_date" IS NULL """);  // только активные
            }
            
            sql.Append($"""
                        ORDER BY "{columnMapping}" {pageRequest.Direction}
                        LIMIT @Count
                        OFFSET @Offset;
                        """);

            var result = await connection
                .QueryAsync<VariantReservationDto>(
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
    
    public async Task<VariantReservationDto?> FindByVariantAndSizeAsync(
        Guid variantId,
        Guid sizeId,
        CancellationToken cancellationToken)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
        
        if (sizeId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sizeId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = 
                $"""
                 {GetBaseSql()}
                 WHERE "variant_id" = @VariantId
                   AND "size_id"    = @SizeId
                   AND "deleted_date" IS NULL
                   AND "expires_date" > NOW();
                 """;

            return await connection
                .QueryFirstOrDefaultAsync<VariantReservationDto>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { VariantId = variantId, SizeId = sizeId },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<VariantReservationDto>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = 
                $"""
                 {GetBaseSql()}
                 WHERE "order_id" = @OrderId
                   AND "deleted_date" IS NULL
                 ORDER BY "created_date" DESC;
                 """;

            var result = await connection
                .QueryAsync<VariantReservationDto>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { OrderId = orderId },
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
    
    public async Task<IReadOnlyList<VariantReservationDto>> FindExpiredReservationsAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = 
                $"""
                 {GetBaseSql()}
                 WHERE "expires_date" < NOW()
                   AND "deleted_date" IS NULL
                   AND "status" != 'Expired'
                 ORDER BY "expires_date" ASC;
                 """;

            var result = await connection
                .QueryAsync<VariantReservationDto>(
                    new CommandDefinition(
                        commandText: sql,
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
                   "id"              AS Id,
                   "quantity"        AS Quantity,
                   "status"          AS Status,
                   "cancel_reason"   AS CancelReason,
                   "created_date"    AS CreatedAt,
                   "updated_date"    AS UpdatedAt,
                   "expires_date"    AS ExpiresAt,
                   "deleted_date"    AS DeletedAt,
                   "variant_id"      AS VariantId,
                   "size_id"         AS SizeId,
                   "order_id"        AS OrderId
               FROM "reservations"
               """;
    }
}