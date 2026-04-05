using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Domain.ReadModels.Product.FullPage;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class FullProductQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      RenStore.Catalog.Application.Abstractions.Queries.IFullProductQuery
{
    public FullProductQuery(
        ILogger<FullProductQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }
    
    public async Task<FullProductPageDto?> FindFullAsync(
        Guid variantId,
        CancellationToken cancellationToken)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = GetFullProductSql();

            await using var result = await connection
                .QueryMultipleAsync(
                    sql: sql, 
                    param: new { Id = variantId }, 
                    transaction: CurrentDbTransaction,
                    commandTimeout: CommandTimeoutSeconds);

            var product = await result.ReadFirstOrDefaultAsync<ProductDto>();
            var variant = await result.ReadFirstOrDefaultAsync<ProductVariantDto>();
            
            if (variant is null || product is null) return null;
            
            var details = await result.ReadFirstOrDefaultAsync<VariantDetailsDto>();
            var images = (await result.ReadAsync<ProductVariantImageDto>()).ToList();
            var attributes = (await result.ReadAsync<VariantAttributeDto>()).ToList();
            var sizes = (await result.ReadAsync<VariantSizeDto>()).ToList();
            var prices = (await result.ReadAsync<PriceHistoryDto>()).ToList();
            var otherVariants = (await result.ReadAsync<ProductVariantLinkDto>()).ToList();

            var priceDic = prices.ToDictionary(p => p.SizeId);

            var sizesWithPrices = sizes
                .Select(s =>
                    new SizeDto(
                        Size: s,
                        History: priceDic.GetValueOrDefault(s.SizeId)
                    ))
                .ToList();
            
            var fullPage = new FullProductPageDto()
            {
                Product = product,
                Variant = variant,
                Details = details,
                Images = images,
                Attributes = attributes,
                SizeWithPrices = sizesWithPrices,
                OtherVariantsLinks = otherVariants
            };

            return fullPage;
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    private static string GetFullProductSql()
    {
        const string productSql =
            @"
                SELECT 
                    p.""id""                    AS ProductId,
                    p.""status""                AS Status,
                    p.""seller_id""             AS SellerId,
                    p.""category_id""           AS CategoryId,
                    p.""sub_category_id""       AS SubCategoryId
                FROM ""products""               AS p
                INNER JOIN ""product_variants"" AS pv
                    ON p.""id"" = pv.""product_id""
                WHERE pv.""id"" = @Id
                AND p.""status"" = 'published'
                LIMIT 1;
            ";
        
        const string variantSql =
            @"
                SELECT 
                    pv.""id""              AS VariantId,
                    pv.""name""            AS Name,
                    pv.""article""         AS Article,
                    pv.""status""          AS Status,
                    pv.""url""             AS Url,
                    pv.""main_image_id""   AS MainImageId,
                    pv.""size_system""     AS SizeSystem,
                    pv.""size_type""       AS SizeType,
                    pv.""color_id""        AS ColorId
                FROM ""product_variants""  AS pv
                WHERE pv.""id"" = @Id
                AND pv.""status"" = 'published';
            ";
        
        const string detailsSql =
            @" 
                SELECT 
                    d.""id""                  AS DetailsId,
                    d.""description""         AS Description,
                    d.""composition""         AS Composition,
                    d.""model_features""      AS ModelFeatures,
                    d.""decorative_elements"" AS DecorativeElements,
                    d.""equipment""           AS Equipment,
                    d.""caring_of_things""    AS CaringOfThings,
                    d.""type_of_packing""     AS TypeOfPacking
                FROM ""variant_details""      AS d
                WHERE d.""variant_id"" = @Id
                LIMIT 1;
            ";

        const string images =
            @"
                SELECT 
                    image.""id""           AS ImageId,
                    image.""storage_path"" AS StoragePath,
                    image.""is_main""      AS IsMain,
                    image.""sort_order""   AS SortOrder,
                    image.""variant_id""   AS VariantId
                FROM ""variant_images""    AS image
                WHERE image.""variant_id"" = @Id
                AND image.""is_deleted"" = false
                ORDER BY image.""sort_order"" ASC;
            ";
        
        const string attributesSql =
            @" 
                SELECT 
                    a.""id""                AS AttributeId,
                    a.""key""               AS Key,
                    a.""value""             AS Value
                FROM ""variant_attributes"" AS a
                WHERE a.""variant_id"" = @Id
                AND a.""is_deleted"" = false;
            ";
        
        const string sizeSql =
            @" 
                SELECT 
                    size.""id""          AS SizeId,
                    size.""letter_size"" AS LetterSize,
                    size.""size_number"" AS Number,
                    size.""type""        AS Type,
                    size.""size_system"" AS System
                FROM ""variant_sizes""   AS size
                WHERE size.""variant_id"" = @Id
                AND size.""is_deleted"" = false
                ORDER BY size.""size_number"" ASC;
            ";
        
        const string priceSql =
            @"
                SELECT 
                    price.""id""             AS PriceId,
                    price.""price""          AS Amount,
                    price.""currency""       AS Currency,
                    price.""size_id""        AS SizeId
                FROM ""price_history""       AS price
                INNER JOIN ""variant_sizes"" AS s
                    ON price.""size_id"" = s.""id""
                WHERE s.""variant_id"" = @Id
                AND price.""is_active"" = true;
            ";

        const string otherVariants =
            @"
                SELECT 
                    pv.""id""                 AS VariantId,
                    pv.""url""                AS Url,
                    pv.""main_image_id""      AS MainImageId,
                    image.""storage_path""    AS StoragePath
                FROM ""product_variants""     AS pv
                LEFT JOIN ""variant_images"" AS image
                    ON pv.""main_image_id"" = image.""id""
                WHERE pv.""product_id"" = (
                    SELECT ""product_id""
                    FROM ""product_variants""
                    WHERE ""id"" = @Id
                )
                AND pv.""id"" != @Id
                AND pv.""status"" = 'published';
            ";
        
        return string.Join("\n", 
            productSql, 
            variantSql, 
            detailsSql, 
            images, 
            attributesSql,
            sizeSql, 
            priceSql,
            otherVariants);
    }
}