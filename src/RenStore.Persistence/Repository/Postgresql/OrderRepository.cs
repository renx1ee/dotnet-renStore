using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderRepository> _logger;
    private readonly string _connectionString;

    private readonly Dictionary<OrderSortBy, string> _sortColumnMapping = new()
    {
        { OrderSortBy.Id, "order_id" }
    };
    
    public OrderRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public OrderRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        var result = await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        var existingOrder = await this.GetByIdAsync(order.Id, cancellationToken);
        
        _context.Orders.Update(existingOrder);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await this.GetByIdAsync(id, cancellationToken);
        this._context.Orders.Remove(order);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_id");

            string sql =
                @$" 
                    SELECT 
                        ""order_id""            AS Id,
                        ""total_price""         AS TotalPrice,
                        ""sub_total_price""     AS SubTotalPrice,
                        ""tax_amount""          AS TaxAmount,
                        ""status""              AS Status,
                        ""cancellation_reason"" AS CancellationReason,
                        ""created_date""        AS CreatedAt,
                        ""updated_date""        AS UpdatedAt,
                        ""shipped_date""        AS ShippedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""user_id""             AS UserId,
                        ""promo_code_id""       AS PromoCodeId
                    FROM
                        ""orders""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset
                ";

            return await connection
                .QueryAsync<OrderEntity>(
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

    public async Task<OrderEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @$" 
                    SELECT 
                        ""order_id""            AS Id,
                        ""total_price""         AS TotalPrice,
                        ""sub_total_price""     AS SubTotalPrice,
                        ""tax_amount""          AS TaxAmount,
                        ""status""              AS Status,
                        ""cancellation_reason"" AS CancellationReason,
                        ""created_date""        AS CreatedAt,
                        ""updated_date""        AS UpdatedAt,
                        ""shipped_date""        AS ShippedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""user_id""             AS UserId,
                        ""promo_code_id""       AS PromoCodeId
                    FROM
                        ""orders""
                    WHERE
                        ""order_item_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<OrderEntity>(
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

    public async Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(OrderEntity), id);
    }

    public async Task<IEnumerable<OrderEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_id");
            

            string sql =
                @$" 
                    SELECT 
                        ""order_id""            AS Id,
                        ""total_price""         AS TotalPrice,
                        ""sub_total_price""     AS SubTotalPrice,
                        ""tax_amount""          AS TaxAmount,
                        ""status""              AS Status,
                        ""cancellation_reason"" AS CancellationReason,
                        ""created_date""        AS CreatedAt,
                        ""updated_date""        AS UpdatedAt,
                        ""shipped_date""        AS ShippedAt,
                        ""cancelled_date""      AS CancelledAt,
                        ""user_id""             AS UserId,
                        ""promo_code_id""       AS PromoCodeId
                    FROM
                        ""orders""
                    WHERE
                        ""user_id"" = @Id
                    ORDER BY {columnName} {direction}
                    LIMIT @Limit
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<OrderEntity>(
                    sql, new
                    {
                        Id = userId,
                        Count = pageCount,
                        Offset = offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<OrderEntity>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByUserIdAsync(
            userId: userId, 
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageCount: pageCount,
            page: page,
            descending: descending);

        if(result is null || !result.Any())
               throw new NotFoundException(typeof(OrderEntity), userId);

        return result;
    }
}