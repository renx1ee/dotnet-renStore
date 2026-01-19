using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Domain.DTOs.Product.FullPage;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<ProductRepository> _logger;
    private readonly Dictionary<ProductSortBy, string> _sortColumnMapping = new()
    {
        { ProductSortBy.Id, "product_id" }
    };

    public ProductRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<Guid> CreateAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var result = await _context.Products.AddAsync(product, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var existingProduct = await this.GetByIdAsync(product.Id, cancellationToken);
        this._context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var existingProduct = await this.GetByIdAsync(id, cancellationToken);
        this._context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        ProductSortBy sortBy = ProductSortBy.Id,
        bool? isBlocked = null)
    {
        try
        {
            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_id");
            
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            StringBuilder sql = new StringBuilder(
                $@"
                    SELECT
                        ""product_id""     AS Id,
                        ""is_blocked""     AS IsBlocked,
                        ""overall_rating"" AS OverallRating,
                        ""seller_id""      AS SellerId,
                        ""category_id""    AS CategoryId
                    FROM
                        ""products""
                    
                ");

            if (isBlocked.HasValue)
                sql.Append(@$" WHERE ""is_blocked"" = {isBlocked.Value}");

            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");

            return await connection
                .QueryAsync<Product>(
                    sql.ToString(), new
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
    
    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                        ""product_id""     AS Id,
                        ""is_blocked""     AS IsBlocked,
                        ""overall_rating"" AS OverallRating,
                        ""seller_id""      AS SellerId,
                        ""category_id""    AS CategoryId
                    FROM
                        ""products""
                    WHERE
                        ""product_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<Product>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(Product), id);
    }
    
    public async Task<ProductFullDto?> FindFullAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            string sql = GetFullProductSql();

            await using var result = await connection.QueryMultipleAsync(sql, new { Id = id }, commandTimeout: 30);

            var product = await result.ReadFirstOrDefaultAsync<ProductDto>();
            if (product is null) return null;

            var fullProduct = new ProductFullDto()
            {
                Product = product,
                Variants = (await result.ReadAsync<ProductVariantDto>()).ToList(),
                Details = (await result.ReadAsync<ProductDetailDto>()).ToList(),
                Cloth = await result.ReadFirstOrDefaultAsync<ProductClothDto>(),
                ClothSizes = (await result.ReadAsync<ProductClothSizeDto>()).ToList(),
                Attributes = (await result.ReadAsync<ProductAttributeDto>()).ToList(),
                Prices = (await result.ReadAsync<ProductPriceHistoryDto>()).ToList(),
                Seller = await result.ReadFirstOrDefaultAsync<SellerDto>(),
            };

            fullProduct.Details ??= new List<ProductDetailDto>();
            fullProduct.ClothSizes ??= new List<ProductClothSizeDto>();
            fullProduct.Variants ??= new List<ProductVariantDto>();
            fullProduct.Attributes ??= new List<ProductAttributeDto>();
            fullProduct.Prices ??= new List<ProductPriceHistoryDto>();

            return fullProduct;
        }
        catch (PostgresException e)
        {
            throw new InvalidOperationException($"Database error occured: {e.Message}");
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error occured: {e.Message}");
        }
    }
    
    private static string GetFullProductSql()
    {
        const string productSql = 
            @"
                SELECT 
                    p.""product_id""               AS Id, 
                    p.""is_blocked""               AS IsBlocked, 
                    p.""overall_rating""           AS OverallRating, 
                    p.""seller_id""                AS SellerId, 
                    p.""category_id""              AS CategoryId
                FROM ""products""                  AS p
                WHERE p.""product_id"" = @Id;
            ";
            
        const string variantSql = 
            @"
                SELECT
                    pv.""product_variant_id""      AS VariantId,
                    pv.""variant_name""            AS Name,
                    pv.""normalized_variant_name"" AS NormalizedName,
                    pv.""rating""                  AS Rating,
                    pv.""article""                 AS Article,
                    pv.""in_stock""                AS InStock,
                    pv.""is_available""            AS IsAvailable,
                    pv.""created_date""            AS OccuredAt,
                    pv.""url""                     AS Url,
                    pv.""product_id""              AS ProductId,
                    pv.""color_id""                AS ColorId
                FROM ""product_variants""          AS pv
                WHERE ""product_id"" = @Id
                ORDER BY ""created_date"";
            ";
        
        const string detailSql = 
            @"
                SELECT
                    pd.""product_detail_id""       AS DetailId,
                    pd.""description""             AS Description,
                    pd.""model_features""          AS ModelFeatures,
                    pd.""decorative_elements""     AS DecorativeElements,
                    pd.""equipment""               AS Equipment,
                    pd.""composition""             AS Composition,
                    pd.""caring_of_things""        AS CaringOfThings,
                    pd.""type_of_packing""         AS TypeOfPacking,
                    pd.""country_id""              AS CountryOfManufactureId,
                    pd.""product_variant_id""      AS ProductVariantId,
                    countries.""country_name""     AS Name
                FROM ""product_details""           AS pd
                LEFT JOIN ""countries""
                    ON ""countries"".""country_id"" = pd.""country_id""
                INNER JOIN ""product_variants""    AS pv
                    ON pv.""product_variant_id"" = pd.""product_variant_id""
                WHERE pv.""product_id"" = @Id; 
            ";
        
        const string clothSql = 
            @"
                SELECT
                    pc.""product_cloth_id""        AS ClothId,
                    pc.""gender""                  AS Gender,
                    pc.""season""                  AS Season,
                    pc.""neckline""                AS Neckline,
                    pc.""the_cut""                 AS TheCut
                FROM ""product_clothes""           AS pc                                 
                WHERE pc.""product_id"" = @Id;   
            ";
        
        const string clothSizeSql = 
            @"
                SELECT
                    pcs.""cloth_size_id""          AS ClothSizeId,
                    pcs.""amount""                 AS Amount,
                    pcs.""cloth_size""             AS ClothSize,
                    pcs.""product_cloth_id""       AS ProductClothId
                FROM ""product_cloth_sizes""       AS pcs                  
                INNER JOIN ""product_clothes""     AS pc
                    ON pc.product_cloth_id = pcs.product_cloth_id                              
                WHERE pc.""product_id"" = @Id;
            ";
        
        const string attributeSql = 
            @"
                SELECT 
                    pa.""attribute_id""            AS AttributeId,
                    pa.""attribute_name""          AS Name,
                    pa.""attribute_value""         AS Value,
                    pa.""product_variant_id""      AS ProductVariantId
                FROM ""product_attributes""        AS pa
                INNER JOIN ""product_variants""    AS pv
                    ON pv.""product_variant_id"" = pa.""product_variant_id""
                WHERE pv.""product_id"" = @Id;
            ";
        
        const string priceSql = 
            @"
                SELECT
                    ph.""price_history_id""        AS PriceHistoryId,
                    ph.""price""                   AS Price,
                    ph.""old_price""               AS OldPrice,
                    ph.""discount_price""          AS DiscountPrice,
                    ph.""discount_percent""        AS DiscountPercent,
                    ph.""start_date""              AS StartDate,
                    ph.""end_date""                AS EndDate,
                    ph.""changed_by""              AS ChangedBy,
                    ph.""product_variant_id""      AS ProductVariantId
                FROM ""product_price_histories""   AS ph
                INNER JOIN ""product_variants""    AS pv
                    ON pv.""product_variant_id"" = ph.""product_variant_id""
                WHERE pv.""product_id"" = @Id
                ORDER BY ph.""start_date"" DESC;
            ";
        
        const string sellerSql = 
            @"
                SELECT 
                    s.""seller_id""                AS SellerId,
                    s.""seller_name""              AS Name,
                    s.""url""                      AS Url
                FROM ""sellers""                   AS s
                WHERE ""seller_id"" = (
                    SELECT ""seller_id""
                    FROM ""products""
                    WHERE ""product_id"" = @Id);
            ";
        
        return string.Join("\n", productSql, variantSql, detailSql, clothSql, clothSizeSql, attributeSql, priceSql, sellerSql);
    }
}