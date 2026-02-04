using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductPriceHistoryRepository : IProductPriceHistoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductPriceHistorySortBy, string> _sortColumnMapping = new()
    {
        { ProductPriceHistorySortBy.Id, "price_history_id" }
    };

    public ProductPriceHistoryRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductPriceHistoryRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(
        ProductPriceHistory priceHistory, 
        CancellationToken cancellationToken)
    {
        var result = await _context.PriceHistories.AddAsync(priceHistory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        ProductPriceHistory priceHistory,
        CancellationToken cancellationToken)
    {
        var existingAttribute = await this.GetByIdAsync(priceHistory.Id, cancellationToken);

        _context.PriceHistories.Update(priceHistory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var priceHistory = await this.GetByIdAsync(id, cancellationToken);
        
        _context.PriceHistories.Remove(priceHistory);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductPriceHistory>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductPriceHistorySortBy sortBy = ProductPriceHistorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string direction = descending ? "DESC" : "ASC";
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "price_history_id");

            string sql =
                $@"
                    SELECT 
                        ""price_history_id""   AS Id,
                        ""price""              AS Price,
                        ""old_price""          AS OldPrice,
                        ""discount_price""     AS DiscountPrice,
                        ""discount_percent""   AS DiscountPercent,
                        ""start_date""         AS StartDate,
                        ""end_date""           AS EndDate,
                        ""changed_by""         AS ChangedBy,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_price_histories""
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductPriceHistory>(
                    sql, new
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

    public async Task<ProductPriceHistory?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""price_history_id""   AS Id,
                        ""price""              AS Price,
                        ""old_price""          AS OldPrice,
                        ""discount_price""     AS DiscountPrice,
                        ""discount_percent""   AS DiscountPercent,
                        ""start_date""         AS StartDate,
                        ""end_date""           AS EndDate,
                        ""changed_by""         AS ChangedBy,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_price_histories""
                    WHERE 
                        ""price_history_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductPriceHistory>(
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

    public async Task<ProductPriceHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductPriceHistory), id);
    }
}