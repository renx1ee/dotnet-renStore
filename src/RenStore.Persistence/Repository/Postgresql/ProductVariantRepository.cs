using System.Text;
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

public class ProductVariantRepository : IProductVariantRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductVariantSortBy, string> _sortColumnMapping = new()
    {
        { ProductVariantSortBy.Id, "product_variant_id" },
        { ProductVariantSortBy.Name, "normalized_variant_name" }
    };

    public ProductVariantRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductVariantRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<Guid> CreateAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken)
    {
        var result = await _context.ProductVariants.AddAsync(productVariant, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken)
    {
        var existingProduct = await this.GetByIdAsync(productVariant.Id, cancellationToken);
        this._context.ProductVariants.Update(productVariant);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var existingProduct = await this.GetByIdAsync(id, cancellationToken);
        this._context.ProductVariants.Remove(existingProduct);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductVariant>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        bool? isAvailable = null)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_variant_id");
            
            var sql = new StringBuilder(
                $@"
                    SELECT
                        ""product_variant_id""      AS Id,
                        ""variant_name""            AS Key,
                        ""normalized_variant_name"" AS NormalizedName,
                        ""rating""                  AS Rating,
                        ""article""                 AS Article,
                        ""in_stock""                AS InStock,
                        ""is_available""            AS IsAvailable,
                        ""created_date""            AS OccuredAt,
                        ""product_id""              AS ProductId,
                        ""color_id""                AS ColorId
                    FROM
                        ""product_variants""
                ");

            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);

            if (isAvailable.HasValue)
            {
                sql.Append(@" WHERE ""is_available"" = @IsAvailable");
                parameters.Add("IsAvailable", isAvailable.Value);
            }

            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
            
            
            return await connection
                .QueryAsync<ProductVariant>(
                    sql.ToString(),
                    parameters);
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<ProductVariant?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT
                        ""product_variant_id""      AS Id,
                        ""variant_name""            AS Key,
                        ""normalized_variant_name"" AS NormalizedName,
                        ""rating""                  AS Rating,
                        ""article""                 AS Article,
                        ""in_stock""                AS InStock,
                        ""is_available""            AS IsAvailable,
                        ""created_date""            AS OccuredAt,
                        ""product_id""              AS ProductId,
                        ""color_id""                AS ColorId
                    FROM
                        ""product_variants""
                    WHERE
                        ""product_variant_id"" = @Id
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductVariant>(
                    sql, new
                    {
                        Id = id
                    });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ProductVariant> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(Product), id);
    }
}