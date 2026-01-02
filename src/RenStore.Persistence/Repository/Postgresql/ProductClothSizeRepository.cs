using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductClothSizeRepository : IProductClothSizeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductClothSizeSortBy, string> _sortColumnMapping = new()
    {
        { ProductClothSizeSortBy.Id, "cloth_size_id" }
    };

    public ProductClothSizeRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductClothSizeRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(
        ProductClothSizeEntity clothSize, 
        CancellationToken cancellationToken)
    {
        var result = await _context.ProductClothSizes.AddAsync(clothSize, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        ProductClothSizeEntity clothSize,
        CancellationToken cancellationToken)
    {
        var existingClothSize = await this.GetByIdAsync(clothSize.Id, cancellationToken);

        _context.ProductClothSizes.Update(clothSize);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var clothSize = await this.GetByIdAsync(id, cancellationToken);
        
        _context.ProductClothSizes.Remove(clothSize);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductClothSizeEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductClothSizeSortBy sortBy = ProductClothSizeSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_cloth_id");

            string sql =
                $@"
                    SELECT 
                        ""cloth_size_id""    AS Id,
                        ""cloth_size""       AS ClothSizes,
                        ""amount""           AS Amount,
                        ""product_cloth_id"" AS ProductClothId
                    FROM
                        ""product_cloth_sizes""
                    ORDER BY 
                        {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductClothSizeEntity>(
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

    public async Task<ProductClothSizeEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""cloth_size_id""    AS Id,
                        ""cloth_size""       AS ClothSizes,
                        ""amount""           AS Amount,
                        ""product_cloth_id"" AS ProductClothId
                    FROM
                        ""product_cloth_sizes""
                    WHERE 
                        ""cloth_size_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductClothSizeEntity>(
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

    public async Task<ProductClothSizeEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductClothSizeEntity), id);
    }
}