namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class CatalogQuery(CatalogDbContext context, ILogger logger) 
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase(context, logger),
      RenStore.Catalog.Application.Abstractions.Queries.ICatalogQuery
{
    private const string BaseSqlQuery =
        """
        SELECT
            pv."id"            AS VariantId,
            pv."article"       AS Article,
            pv."url"           AS VariantUrlSlug,
            pv."name"          AS Name,
            pv."created_date"  AS CreatedDate,
            img."id"           AS ImageId,
            img."storage_path" AS StoragePath,
            -- img."url"       AS ImageUrlSlug,
            price."price"      AS Amount,
            price."currency"   AS Currency
        FROM "product_variants" pv
        -- image
        LEFT JOIN LATERAL (
            SELECT 
                pi."id",
                pi."storage_path"
            FROM "variant_images" pi
            WHERE pi."variant_id" = pv."id"
            AND pi."is_deleted" = false
            ORDER BY pi."is_main" DESC
            LIMIT 1
        ) img ON true
        -- price  
        LEFT JOIN LATERAL (
            SELECT 
                ph."price", 
                ph."currency" 
            FROM "variant_sizes" vs
            JOIN "price_history" ph
                ON ph."size_id" = vs."id"
            WHERE vs."variant_id" = pv."id"
            AND ph."is_active" = true
            ORDER BY ph."price" ASC,
                     ph."created_date" DESC
            LIMIT 1
        ) price ON true
        INNER JOIN "products" p 
              ON p."id" = pv."product_id"
        WHERE pv."status" = 'published' 
        AND p."status" = 'published'
        """;
    
    private static readonly Dictionary<CatalogFilterSortBy, string> _sortColumnMapping = new()
    {
        { CatalogFilterSortBy.Popular,   """pv."sales_count" """ },
        { CatalogFilterSortBy.Rating,    """pv."average_rating" """ },
        { CatalogFilterSortBy.PriceAsc,  """price."price" """ },
        { CatalogFilterSortBy.PriceDesc, """price."price" """ },
        { CatalogFilterSortBy.Newest,    """pv."created_date" """ }
    };
    
    public async Task<IReadOnlyList<CatalogReadModel>> SearchAsync(
        CatalogSearchFilter filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            if (!_sortColumnMapping.TryGetValue(filter.SortBy, out var columnMapping))
                throw new ArgumentOutOfRangeException(nameof(filter.SortBy));
            
            var pageRequest = BuildPageRequest(
                page: filter.Page, 
                pageSize: filter.PageSize, 
                descending: filter.Descending);

            var direction = filter.SortBy == CatalogFilterSortBy.PriceAsc ? "ASC"
                : filter.SortBy == CatalogFilterSortBy.PriceDesc ? "DESC"
                : pageRequest.Direction;

            var sql = new StringBuilder();
            
            sql.Append(
                $"""
                {BaseSqlQuery}
                """);
            
            var parameters = new DynamicParameters();
            parameters.Add("Limit", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (filter.CategoryId.HasValue)
            {
                sql.Append(""" AND p."category_id" = @CategoryId""");
                parameters.Add("CategoryId", filter.CategoryId);
            }
            
            if (filter.SubCategoryId.HasValue)
            {
                sql.Append(""" AND p."sub_category_id" = @SubCategoryId""");
                parameters.Add("SubCategoryId", filter.SubCategoryId);
            }
            
            if (filter.MaxPrice.HasValue)
            {
                sql.Append(""" AND price."price" <= @MaxPrice """);
                parameters.Add("MaxPrice", filter.MaxPrice);
            }
            
            if (filter.MinPrice.HasValue)
            {
                sql.Append(""" AND price."price" >= @MinPrice """);
                parameters.Add("MinPrice", filter.MinPrice);
            }
            
            if (filter.ColorId.HasValue)
            {
                sql.Append(""" AND pv."color_id" = @ColorId """);
                parameters.Add("ColorId", filter.ColorId);
            }
            
            if (filter.HasDiscount)
            {
                sql.Append("""
                           AND pv."discount_percents" IS NOT NULL 
                           AND pv."discount_percents" > 0
                           """);
                
                if (filter.MinDiscountPercents.HasValue)
                {
                    sql.Append(""" AND pv."discount_percents" >= @MinDiscountPercents """); //TODO: 
                    parameters.Add("MinDiscountPercents", filter.MinDiscountPercents);
                }
            }
            // TODO: сделать наличие по конкретным размерам
            if (filter.IsAvailable.HasValue)
            {
                sql.Append(
                    filter.IsAvailable == true
                        ? """ AND pv."in_stock" > 0 """ 
                        : """ AND pv."in_stock" <= 0 """
                    );
            }
            // TODO: 
            if (filter.MinReviewsCount.HasValue)
            {
                sql.Append(""" AND pv."reviews_count" >= @MinReviewsCount """);
                parameters.Add("MinReviewsCount", filter.MinReviewsCount);
            }
            
            if (filter.MinAverageRating.HasValue)
            {
                sql.Append(""" AND pv."average_rating" >= @MinAverageRating """);
                parameters.Add("MinAverageRating", filter.MinAverageRating);
            }
            
            /*if (filter.SelesCount.HasValue)
            {
                sql.Append(""" AND pv."sales_count" = @SelesCount """);
                parameters.Add("SelesCount", filter.SelesCount);
            }*/
            
            if (filter.OnlyVerifiedSellers)
            {
                sql.Append(""" AND pv."is_verified_seller" = true """);
            }
            
            if (!string.IsNullOrEmpty(filter.Search))
            {
                sql.Append(""" AND pv."normalized_name" ILIKE @Name """);
                parameters.Add("Name", $"%{filter.Search}%");
            }
            
            sql.Append(
                $"""
                ORDER BY {columnMapping} {direction}, pv."id"
                LIMIT @Limit
                OFFSET @Offset; 
                """);
            
            var result = await connection
                .QueryAsync<CatalogReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: parameters,
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
}