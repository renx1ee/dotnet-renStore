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

namespace RenStore.Delivery.Persistence.Repositories;

public class DeliveryOrderQuery
    (ILogger<DeliveryOrderQuery> logger,
    ApplicationDbContext context)
    : RenStore.Delivery.Application.Interfaces.IDeliveryOrderQuery
{
    private const uint MaxPageSize = 1000;
    private const int CommandTimeoutSeconds = 30;
    
    private const string BaseSqlQuery =
        """
            SELECT
                ""delivery_order_id""             AS Id,
                ""created_date""                  AS CreatedAt,
                ""delivered_date""                AS DeliveredAt,
                ""deleted_date""                  AS DeletedAt,
                ""status""                        AS Status,
                ""current_sorting_center_id""     AS CurrentSortingCenterId,
                ""destination_sorting_center_id"" AS DestinationSortingCenterId,
                ""pickup_point_id""               AS PickupPointId,
                ""order_id""                      AS OrderId,
                ""address_id""                    AS AddressId,
                ""delivery_tariff_id""            AS DeliveryTariffId
            FROM
                ""delivery_orders""
        """;
    
    private readonly ILogger<DeliveryOrderQuery> _logger = logger
                                                           ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context       = context
                                                           ?? throw new ArgumentNullException(nameof(context));
    
    private static readonly Dictionary<DeliveryOrderSortBy, string> _sortColumnMapping =new()
    {
        { DeliveryOrderSortBy.Id, "delivery_order_id"},
        { DeliveryOrderSortBy.CreatedAt, "created_date"},
        { DeliveryOrderSortBy.DeliveredAt, "delivered_date"},
        { DeliveryOrderSortBy.DeletedAt, "deleted_date"}
    };
    
    private DbTransaction? CurrentDbTransaction =>
        this._context.Database.CurrentTransaction?.GetDbTransaction();
    
    public async Task<IReadOnlyList<DeliveryOrderReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
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
                .QueryAsync<DeliveryOrderReadModel>(
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

    public async Task<DeliveryOrderReadModel?> FindByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be Guid Empty.");
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            string sql =
                $@" 
                    {BaseSqlQuery}
                    WHERE """" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryOrderReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));

        }
        catch (Exception e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<DeliveryOrderReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id: id, cancellationToken: cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryOrderReadModel), id);
    }

    public async Task<IReadOnlyList<DeliveryOrderReadModel>> FindByOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId));
        
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
                    WHERE ""order_id"" = @Id
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<DeliveryOrderReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Id = orderId,
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

    public async Task<IReadOnlyList<DeliveryOrderReadModel>> GetByOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByOrderId(
            orderId: orderId,
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            page: page,
            pageSize: pageSize,
            descending: descending,
            isDeleted: isDeleted);

        if (result.Count == 0)
            throw new NotFoundException(typeof(DeliveryOrderReadModel), orderId);

        return result;
    }
    
    public async Task<IReadOnlyList<DeliveryOrderReadModel>> FindByDeliveryTariffId(
        Guid tariffId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (tariffId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(tariffId));
        
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
                    WHERE ""delivery_tariff_id"" = @Id
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<DeliveryOrderReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Id = tariffId,
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

    public async Task<IReadOnlyList<DeliveryOrderReadModel>> GetByDeliveryTariffId(
        Guid tariffId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByOrderId(
            orderId: tariffId,
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            page: page,
            pageSize: pageSize,
            descending: descending,
            isDeleted: isDeleted);

        if (result.Count == 0)
            throw new NotFoundException(typeof(DeliveryOrderReadModel), tariffId);

        return result;
    }

    private async Task<DbConnection> GetOpenDbConnectionAsync(
        CancellationToken cancellationToken)
    {
        var connection = this._context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }

    private DeliveryOrderPageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
    {
        if (page == 0)
            throw new ArgumentOutOfRangeException(nameof(page));
        
        pageSize = Math.Min(pageSize, MaxPageSize);
        var offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";
        
        return new DeliveryOrderPageRequest(
            Limit: (int)pageSize,
            Offset: (int)offset,
            Direction: direction);
    }

    private Exception Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occurred.");
        return new Exception("Database error occurred.", e);
    }
}

readonly record struct DeliveryOrderPageRequest(int Limit, int Offset, string Direction);
