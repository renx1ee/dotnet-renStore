using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Filters;
using RenStore.Catalog.Contracts.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;

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
            -- , ph."created_date" DESC
            LIMIT 1
        ) price ON true
        INNER JOIN "products" p 
              ON p."id" = pv."product_id"
        WHERE pv."status" = 'published' 
        AND p."status" = 'published'
        """;
    
    private static readonly Dictionary<CatalogFilterSortBy, string> _sortColumnMapping = new()
    {
        { CatalogFilterSortBy.Name,      "normalized_name" },
        { CatalogFilterSortBy.PriceAsc,  "price" },
        { CatalogFilterSortBy.PriceDesc, "price" },
        { CatalogFilterSortBy.Newest,    "created_date" }
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
            parameters.Add("Count", pageRequest.Limit);
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
            
            if (!string.IsNullOrEmpty(filter.Search))
            {
                sql.Append(""" AND pv."normalized_name" ILIKE @Name """);
                parameters.Add("Name", $"%{filter.Search}%");
            }
            
            sql.Append(
                $"""
                ORDER BY "{columnMapping}" {direction}
                LIMIT @Count
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
            _logger.LogError(
                exception: e, 
                message: "Database error in method: {FindAsync} with {Filter}",
                nameof(SearchAsync),
                filter);

            throw Wrap(e);
        }
    }
}