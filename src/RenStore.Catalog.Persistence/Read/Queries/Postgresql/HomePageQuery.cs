using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

// TODO: CREATE INDEX ON variant_images (variant_id, is_main);
//       CREATE INDEX ON variant_sizes (variant_id);
//       CREATE INDEX ON price_history (size_id, price);

internal sealed class HomePageQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase
{
    private const string BaseSqlQuery =
        """
            SELECT
                pv.""id""            AS Id,
                pv.""name""          AS Name,
                pv.""article""       AS Article,
                
                img.""storage_path"" AS StoragePath,
                img.""weight""       AS Weight,
                img.""height""       AS Height,
                
                price.""price""      AS Amount,
                price.""currency""   AS Currency
            
            FROM ""product_variants"" pv
            
            LEFT JOIN LATERAL (
                SELECT 
                    pi.""storage_path"",
                    pi.""weight"",
                    pi.""height""
                FROM ""variant_images"" pi
                WHERE pi.""variant_id"" = pv.""id""
                ORDER BY pi.""is_main"" DESC
                LIMIT 1
            ) img ON true
                
            LEFT JOIN LATERAL (
                SELECT 
                    ph.""price""      AS "Amount", 
                    ph.""currency""   AS "Currency" 
                FROM ""variant_sizes"" vs
                JOIN ""price_history"" ph
                    ON ph.""size_id"" = vs.""id""
                WHERE vs.""variant_id"" = pv.""id""
                ORDER BY 
                    ph.""is_active""    DESC, 
                    ph.""created_date"" DESC
                LIMIT 1
            ) price ON true
        """;
    
    public HomePageQuery(
        ILogger<PriceHistoryQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                ");

            sql.Append($@" ORDER BY pv.""name"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CatalogHomeItemReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
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
}