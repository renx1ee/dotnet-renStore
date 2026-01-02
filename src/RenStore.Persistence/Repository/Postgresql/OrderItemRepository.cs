using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderItemRepository> _logger;
    private readonly string _connectionString;

    private readonly Dictionary<OrderItemSortBy, string> _sortColumnMapping = new()
    {
        { OrderItemSortBy.Id, "order_item_id" }
    };
    
    public OrderItemRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public OrderItemRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(OrderItemEntity orderItem, CancellationToken cancellationToken)
    {
        var result = await _context.OrderItems.AddAsync(orderItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(OrderItemEntity orderItem, CancellationToken cancellationToken)
    {
        var existingOrderItem = await this.GetByIdAsync(orderItem.Id, cancellationToken);
        
        _context.OrderItems.Update(existingOrderItem);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderItem = await this.GetByIdAsync(id, cancellationToken);
        this._context.OrderItems.Remove(orderItem);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItemEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_item_id");

            string sql =
                @$" 
                    SELECT 
                        ""order_item_id""       AS Id,
                        ""price""               AS Price,
                        ""total_price""         AS TotalPrice,
                        ""amount""              AS Amount,
                        ""created_date""        AS CreatedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""status""              AS Status,
                        ""return_reason""       AS ReturnReason,
                        ""returned_amount""     AS ReturnedAmount,
                        ""warranty_start_date"" AS WarrantyStartDate,
                        ""warranty_end_date""   AS WarrantyEndDate,
                        ""order_id""            AS OrderId,
                        ""product_id""          AS ProductId
                    FROM
                        ""order_items""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset
                ";

            return await connection
                .QueryAsync<OrderItemEntity>(
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

    public async Task<OrderItemEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @$" 
                    SELECT 
                        ""order_item_id""       AS Id,
                        ""price""               AS Price,
                        ""total_price""         AS TotalPrice,
                        ""amount""              AS Amount,
                        ""created_date""        AS CreatedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""status""              AS Status,
                        ""return_reason""       AS ReturnReason,
                        ""returned_amount""     AS ReturnedAmount,
                        ""warranty_start_date"" AS WarrantyStartDate,
                        ""warranty_end_date""   AS WarrantyEndDate,
                        ""order_id""            AS OrderId,
                        ""product_id""          AS ProductId
                    FROM
                        ""order_items""
                    WHERE
                        ""order_item_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<OrderItemEntity>(
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

    public async Task<OrderItemEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(OrderItemEntity), id);
    }

    public async Task<IEnumerable<OrderItemEntity>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_item_id");
            

            string sql =
                @$" 
                    SELECT 
                        ""order_item_id""       AS Id,
                        ""price""               AS Price,
                        ""total_price""         AS TotalPrice,
                        ""amount""              AS Amount,
                        ""created_date""        AS CreatedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""status""              AS Status,
                        ""return_reason""       AS ReturnReason,
                        ""returned_amount""     AS ReturnedAmount,
                        ""warranty_start_date"" AS WarrantyStartDate,
                        ""warranty_end_date""   AS WarrantyEndDate,
                        ""order_id""            AS OrderId,
                        ""product_id""          AS ProductId
                    FROM
                        ""order_items""
                    WHERE
                        ""order_id"" = @Id
                    ORDER BY {columnName} {direction}
                    LIMIT @Limit
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<OrderItemEntity>(
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

    public async Task<IEnumerable<OrderItemEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByOrderIdAsync(
            orderId: orderId, 
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageCount: pageCount,
            page: page,
            descending: descending);

        if(result is null || !result.Any())
               throw new NotFoundException(typeof(OrderItemEntity), orderId);

        return result;
    }
    
    public async Task<IEnumerable<OrderItemEntity>> FindByProductOrderIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_item_id");
            

            string sql =
                @$" 
                    SELECT 
                        ""order_item_id""       AS Id,
                        ""price""               AS Price,
                        ""total_price""         AS TotalPrice,
                        ""amount""              AS Amount,
                        ""created_date""        AS CreatedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""status""              AS Status,
                        ""return_reason""       AS ReturnReason,
                        ""returned_amount""     AS ReturnedAmount,
                        ""warranty_start_date"" AS WarrantyStartDate,
                        ""warranty_end_date""   AS WarrantyEndDate,
                        ""order_id""            AS OrderId,
                        ""product_id""          AS ProductId
                    FROM
                        ""order_items""
                    WHERE
                        ""product_id"" = @Id
                    ORDER BY {columnName} {direction}
                    LIMIT @Limit
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<OrderItemEntity>(
                    sql, new
                    {
                        Id = productId,
                        Count = pageCount,
                        Offset = offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<OrderItemEntity>> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByProductOrderIdAsync(
            productId: productId, 
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageCount: pageCount,
            page: page,
            descending: descending);

        if(result is null || !result.Any())
               throw new NotFoundException(typeof(OrderItemEntity), productId);

        return result;
    }
}