using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Filters;
using RenStore.Catalog.Contracts.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class HomePageQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      RenStore.Catalog.Application.Abstractions.Queries.IHomePageQuery
{
    private readonly Dictionary<CatalogFilterSortBy, string> _sortColumnMapping = new()
    {
        { CatalogFilterSortBy.Name, "normalized_name" },
        { CatalogFilterSortBy.PriceAsc, "price" },
        { CatalogFilterSortBy.PriceDesc, "price" },
        { CatalogFilterSortBy.Newest, "created_date" }
    };
    
    private const string BaseSqlQuery =
        """
        SELECT
            pv."id"            AS VariantId,
            pv."article"       AS Article,
            pv."url"           AS VariantUrlSlug,
            pv."name"          AS Name,
            img."id"           AS ImageId,
            img."storage_path" AS StoragePath,
            -- img."url"       AS ImageUrlSlug,
            price."price"      AS Amount,
            price."currency"   AS Currency
        FROM "product_variants" pv
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
    
    public HomePageQuery(
        ILogger<HomePageQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var pageRequest = BuildPageRequest(page, pageSize, descending);

            var sql = 
                $"""
                    {BaseSqlQuery}
                    ORDER BY pv."name" {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                """;

            var result = await connection
                .QueryAsync<CatalogHomeItemReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
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
    // TODO: 
    public async Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAsync(
        CatalogFilter filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var pageRequest = BuildPageRequest(
                page: filter.Page, 
                pageSize: filter.PageSize, 
                descending: filter.Descending);

            var sql = new StringBuilder();
            
            sql.Append(
                $"""
                {BaseSqlQuery}
                """);
            
            var parameters = new DynamicParameters();

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
            
            sql.Append(
                $"""
                ORDER BY pv."name" {pageRequest.Direction}
                LIMIT @Count
                OFFSET @Offset; 
                """);
            
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);
            
            var result = await connection
                .QueryAsync<CatalogHomeItemReadModel>(
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
                nameof(FindAsync),
                filter);

            throw Wrap(e);
        }
    }
}