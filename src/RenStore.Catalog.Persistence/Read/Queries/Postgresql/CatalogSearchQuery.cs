using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class CatalogSearchQuery 
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
                    ph.""price"", 
                    ph.""currency"" 
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
    
    public CatalogSearchQuery(
        ILogger<CatalogSearchQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<CatalogHomeItemReadModel>> SearchAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = 
                $"""
                     {BaseSqlQuery}
                     ORDER BY pv.""name"" {pageRequest.Direction}
                     LIMIT @Count
                     OFFSET @Offset;
                 """;

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

public sealed class CatalogSearchFilter
{
    public Guid CategoryId { get; set; }
    public string Search { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public bool? PriceDescending { get; set; }
}