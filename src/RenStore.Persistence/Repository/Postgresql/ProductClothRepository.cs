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

public class ProductClothRepository : IProductClothRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductClothSortBy, string> _sortColumnMapping = new()
    {
        { ProductClothSortBy.Id, "product_cloth_id" }
    };

    public ProductClothRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductClothRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(
        ProductCloth cloth, 
        CancellationToken cancellationToken)
    {
        var result = await _context.ProductClothes.AddAsync(cloth, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        ProductCloth cloth,
        CancellationToken cancellationToken)
    {
        var existingCloth = await this.GetByIdAsync(cloth.Id, cancellationToken);

        _context.ProductClothes.Update(cloth);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var cloth = await this.GetByIdAsync(id, cancellationToken);
        
        _context.ProductClothes.Remove(cloth);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductCloth>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductClothSortBy sortBy = ProductClothSortBy.Id,
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
                        ""product_cloth_id"" AS Id,
                        ""gender""           AS Gender,
                        ""season""           AS Season,
                        ""neckline""         AS Neckline,
                        ""the_cut""          AS TheCut,
                        ""product_id""       AS ProductId
                    FROM
                        ""product_clothes""
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductCloth>(
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

    public async Task<ProductCloth?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""product_cloth_id"" AS Id,
                        ""gender""           AS Gender,
                        ""season""           AS Season,
                        ""neckline""         AS Neckline,
                        ""the_cut""          AS TheCut,
                        ""product_id""       AS ProductId
                    FROM
                        ""product_clothes""
                    WHERE 
                        ""product_cloth_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductCloth>(
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

    public async Task<ProductCloth?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductCloth), id);
    }
}