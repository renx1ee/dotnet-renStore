using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

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

            var sql = GetFullProductByVariantIdSql();

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
            var images = (await result.ReadAsync<ProductVariantImageDto>(buffered: false)).ToList();
            var attributes = (await result.ReadAsync<VariantAttributeDto>(buffered: false)).ToList();
            var sizes = (await result.ReadAsync<VariantSizeDto>(buffered: false)).ToList();
            var prices = (await result.ReadAsync<PriceHistoryDto>(buffered: false)).ToList();
            var otherVariants = (await result.ReadAsync<ProductVariantLinkDto>(buffered: false)).ToList();

            var priceDic = prices.ToDictionary(p => p.SizeId, p => p);

            var sizesWithPrices = sizes
                .Select(s =>
                    new SizeDto(
                        Size: s,
                        History: priceDic.GetValueOrDefault(s.SizeId)
                    ))
                .ToList();
            
            return new FullProductPageDto()
            {
                Product = product,
                Variant = variant,
                Details = details,
                Images = images,
                Attributes = attributes,
                SizeWithPrices = sizesWithPrices,
                OtherVariantsLinks = otherVariants
            };
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<FullProductPageDto?> FindFullAsync(
        string urlSlug,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(urlSlug))
            throw new ArgumentOutOfRangeException(nameof(urlSlug));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            const string variantIdSql =
                """
                    SELECT "id"
                    FROM "product_variants"
                    WHERE "url" = @Url
                    AND "status" = 'published'
                    LIMIT 1;
                """;

            var variantId = await connection.QueryFirstOrDefaultAsync<Guid>(
                sql: variantIdSql,
                param: new { Url = urlSlug });

            return variantId != Guid.Empty
                ? await FindFullAsync(variantId, cancellationToken)
                : null;
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<FullProductPageDto?> FindFullAsync(
        long article,
        CancellationToken cancellationToken)
    {
        if (article <= 0)
            throw new ArgumentOutOfRangeException(nameof(article));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            const string variantIdSql =
                """
                    SELECT "id"
                    FROM "product_variants"
                    WHERE "article" = @Article
                    AND "status" = 'published'
                    LIMIT 1;
                """;

            var variantId = await connection.QueryFirstOrDefaultAsync<Guid>(
                sql: variantIdSql,
                param: new { Article = article });

            return variantId != Guid.Empty
                ? await FindFullAsync(variantId, cancellationToken)
                : null;
        }
        catch (PostgresException e)
        {
            _logger.LogError(
                exception: e, 
                message: "Database error while fetching full product for Article: {Article}",
                args: article);
            
            throw Wrap(e);
        }
    }

    private static string GetFullProductByVariantIdSql()
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