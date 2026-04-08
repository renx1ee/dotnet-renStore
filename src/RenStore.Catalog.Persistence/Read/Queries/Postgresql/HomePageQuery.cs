using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Contracts.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class HomePageQuery(CatalogDbContext context, ILogger logger) 
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase(context, logger),
      RenStore.Catalog.Application.Abstractions.Queries.IHomePageQuery
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
    
    public async Task<IReadOnlyList<CatalogReadModel>> FindAllAsync(
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
                .QueryAsync<CatalogReadModel>(
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
    // TODO: сделать поиск по категории
}