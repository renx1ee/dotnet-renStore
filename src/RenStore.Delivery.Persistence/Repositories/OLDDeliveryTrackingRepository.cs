/*using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class IDeliveryTrackingRepository : IDeliveryTrackingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IDeliveryTrackingRepository> _logger;
    private readonly string _connectionString;

    private readonly Dictionary<DeliveryTrackingSortBy, string> _sortColumnMapping = new()
    {
        { DeliveryTrackingSortBy.Id, "delivery_tracking_history_id" }
    };
    
    public IDeliveryTrackingRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public IDeliveryTrackingRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(DeliveryTracking deliveryTracking, CancellationToken cancellationToken)
    {
        var result = await _context.DeliveryTrackingHistory.AddAsync(deliveryTracking, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(DeliveryTracking deliveryTracking, CancellationToken cancellationToken)
    {
        var existingDeliveryTracking = await this.GetByIdAsync(deliveryTracking.Id, cancellationToken);
        
        _context.DeliveryTrackingHistory.Update(existingDeliveryTracking);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deliveryTracking = await this.GetByIdAsync(id, cancellationToken);
        this._context.DeliveryTrackingHistory.Remove(deliveryTracking);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<DeliveryTracking>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            var offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "delivery_tracking_history_id");

            string sql =
                @$" 
                    SELECT 
                        ""delivery_tracking_history_id"" AS Id,
                        ""current_location""             AS CurrentLocation,
                        ""status""                       AS Status,
                        ""created_date""                 AS OccuredAt,
                        ""notes""                        AS Notes,
                        ""delivery_order_id""            AS DeliveryOrderId
                    FROM
                        ""delivery_tracking_history""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset
                ";

            return await connection
                .QueryAsync<DeliveryTracking>(
                    sql, new
                    {
                        Offset = (int)offset,
                        Count = (int)pageCount
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<DeliveryTracking?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @$" 
                    SELECT 
                        ""delivery_tracking_history_id"" AS Id,
                        ""current_location""             AS CurrentLocation,
                        ""status""                       AS Status,
                        ""created_date""                 AS OccuredAt,
                        ""notes""                        AS Notes,
                        ""delivery_order_id""            AS DeliveryOrderId
                    FROM
                         ""delivery_tracking_history""
                    WHERE
                        ""delivery_tracking_history_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryTracking>(
                    sql, new
                    {
                        Id = id
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<DeliveryTracking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryTracking), id);
    }

    public async Task<IEnumerable<DeliveryTracking>> FindByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            var offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "delivery_tracking_history_id");
            

            string sql =
                @$" 
                    SELECT 
                        ""delivery_tracking_history_id"" AS Id,
                        ""current_location""             AS CurrentLocation,
                        ""status""                       AS Status,
                        ""created_date""                 AS OccuredAt,
                        ""notes""                        AS Notes,
                        ""delivery_order_id""            AS DeliveryOrderId
                    FROM 
                        ""delivery_tracking_history""
                    WHERE
                        ""delivery_order_id"" = @Id
                    ORDER BY {columnName} {direction}
                    LIMIT @Limit
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<DeliveryTracking>(
                    sql, new
                    {
                        Id = orderId,
                        Count = pageCount,
                        Offset = offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<DeliveryTracking>> GetByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByDeliveryOrderId(
            orderId: orderId, 
            cancellationToken: cancellationToken,
            pageCount: pageCount,
            page: page,
            descending: descending);

        if(result is null || !result.Any())
               throw new NotFoundException(typeof(DeliveryTracking), orderId);

        return result;
    }
}*/