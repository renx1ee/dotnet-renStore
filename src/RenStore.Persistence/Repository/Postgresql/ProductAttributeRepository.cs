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

public class ProductAttributeRepository : IProductAttributeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductAttributeSortBy, string> _sortColumnMapping = new()
    {
        { ProductAttributeSortBy.Id, "attribute_id" }
    };

    public ProductAttributeRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductAttributeRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration
            .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(
        ProductAttribute attribute, 
        CancellationToken cancellationToken)
    {
        var result = await _context.ProductAttributes.AddAsync(attribute, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        ProductAttribute attribute,
        CancellationToken cancellationToken)
    {
        var existingAttribute = await this.GetByIdAsync(attribute.Id, cancellationToken);

        _context.ProductAttributes.Update(attribute);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var attribute = await this.GetByIdAsync(id, cancellationToken);
        
        _context.ProductAttributes.Remove(attribute);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductAttribute>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductAttributeSortBy sortBy = ProductAttributeSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "attribute_id");

            string sql =
                $@"
                    SELECT 
                        ""attribute_id""       AS Id,
                        ""attribute_name""     AS Key,
                        ""attribute_value""    AS Value,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_attributes""
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductAttribute>(
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

    public async Task<ProductAttribute?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""attribute_id""       AS Id,
                        ""attribute_name""     AS Key,
                        ""attribute_value""    AS Value,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_attributes""
                    WHERE 
                        ""attribute_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductAttribute>(
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

    public async Task<ProductAttribute?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductAttribute), id);
    }
}