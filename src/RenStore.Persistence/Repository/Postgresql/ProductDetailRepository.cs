using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductDetailRepository : IProductDetailRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductDetailSortBy, string> _sortColumnMapping = new()
    {
        { ProductDetailSortBy.Id, "product_detail_id" }
    };

    public ProductDetailRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductDetailRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration .GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<Guid> CreateAsync(
        ProductDetail detail, 
        CancellationToken cancellationToken)
    {
        var result = await _context.ProductDetails.AddAsync(detail, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        ProductDetail detail,
        CancellationToken cancellationToken)
    {
        var existingDetail = await this.GetByIdAsync(detail.Id, cancellationToken);

        _context.ProductDetails.Update(detail);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existingDetail = await this.GetByIdAsync(id, cancellationToken);
        
        _context.ProductDetails.Remove(existingDetail);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductDetail>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_detail_id");

            string sql =
                $@"
                    SELECT 
                        ""product_detail_id""   AS Id,
                        ""description""         AS Description,
                        ""model_features""      AS ModelFeatures,
                        ""decorative_elements"" AS DecorativeElements,
                        ""equipment""           AS Equipment,
                        ""composition""         AS Composition,
                        ""caring_of_things""    AS CaringOfThings,
                        ""type_of_packing""     AS TypeOfPacking,
                        ""country_id""          AS CountryOfManufactureId,
                        ""product_variant_id""  AS ProductVariantId
                    FROM
                        ""product_details""
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductDetail>(
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

    public async Task<ProductDetail?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""product_detail_id""   AS Id,
                        ""description""         AS Description,
                        ""model_features""      AS ModelFeatures,
                        ""decorative_elements"" AS DecorativeElements,
                        ""equipment""           AS Equipment,
                        ""composition""         AS Composition,
                        ""caring_of_things""    AS CaringOfThings,
                        ""type_of_packing""     AS TypeOfPacking,
                        ""country_id""          AS CountryOfManufactureId,
                        ""product_variant_id""  AS ProductVariantId
                    FROM
                        ""product_details""
                    WHERE 
                        ""product_detail_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductDetail>(
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

    public async Task<ProductDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductDetail), id);
    }
}