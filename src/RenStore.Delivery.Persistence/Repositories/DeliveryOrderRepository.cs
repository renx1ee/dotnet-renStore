/*using System.ComponentModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class DeliveryOrderRepository
{
    private readonly ILogger<DeliveryOrderRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<DeliveryOrderSortBy, string> _sortColumnMapping = new()
    {
        { DeliveryOrderSortBy.Id, "delivery_order_id" }
    };
    
    public DeliveryOrderRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public DeliveryOrderRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(DeliveryOrder deliveryOrder, CancellationToken cancellationToken)
    {
        var result = await _context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(DeliveryOrder deliveryOrder, CancellationToken cancellationToken)
    {
        var existingDeliveryOrder = await this.GetByIdAsync(deliveryOrder.Id, cancellationToken);
        
        _context.DeliveryOrders.Update(deliveryOrder);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deliveryOrder = await this.GetByIdAsync(id, cancellationToken);
        this._context.DeliveryOrders.Remove(deliveryOrder);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<DeliveryOrder>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "delivery_order_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""delivery_order_id""  AS Id,
                        ""created_date""       AS CreatedAt,
                        ""delivered_date""     AS DeliveredAt,
                        ""order_id""           AS OrderId,
                        ""address_id""         AS OrderId,
                        ""delivery_tariff_id"" AS DeliveryTariffId
                    FROM
                        ""delivery_orders""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<DeliveryOrder>(sql, new
                {   
                    Count = (int)pageCount,
                    Offset = (int)offset
                });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<DeliveryOrder?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new InvalidEnumArgumentException();
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            string sql =
                @$"
                    SELECT
                        ""delivery_order_id""  AS Id,
                        ""created_date""       AS CreatedAt,
                        ""delivered_date""     AS DeliveredAt,
                        ""order_id""           AS OrderId,
                        ""address_id""         AS OrderId,
                        ""delivery_tariff_id"" AS DeliveryTariffId
                    FROM
                        ""delivery_orders""
                    WHERE
                        ""delivery_order_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryOrder>(
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

    public async Task<DeliveryOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryOrder), id);
    }
    
    public async Task<DeliveryOrder?> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new InvalidOperationException();
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            string sql =
                @$"
                    SELECT
                        ""delivery_order_id""  AS Id,
                        ""created_date""       AS CreatedAt,
                        ""delivered_date""     AS DeliveredAt,
                        ""order_id""           AS OrderId,
                        ""address_id""         AS OrderId,
                        ""delivery_tariff_id"" AS DeliveryTariffId
                    FROM
                        ""delivery_orders""
                    WHERE 
                        ""order_id"" = @OrderId;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryOrder>(sql, new
                {   
                    OrderId = orderId
                });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<DeliveryOrder> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await this.FindByOrderIdAsync(orderId, cancellationToken) 
                     ?? throw new NotFoundException(typeof(DeliveryOrder), orderId);
    }
}*/