using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class VariantDetailQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IVariantDetailQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"                  AS Id,
                "description"         AS Description,
                "composition"         AS Composition,
                "model_features"      AS ModelFeatures,
                "decorative_elements" AS DecorativeElements, 
                "equipment"           AS Equipment,
                "caring_of_things"    AS CaringOfThings,
                "type_of_packing"     AS TypeOfPacking,
                "created_date"        AS CreatedAt,
                "updated_date"        AS UpdatedAt,
                "version"             AS Version,
                "country_id"          AS CountryOfManufactureId,
                "variant_id"          AS VariantId
            FROM
                ""variant_details""
        """;

    private readonly Dictionary<ProductDetailSortBy, string> _sortColumnMapping = new()
    {
        { ProductDetailSortBy.Id,      "id" },
        { ProductDetailSortBy.Version, "version" }
    };
    
    public VariantDetailQuery(
        ILogger<VariantDetailQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<VariantDetailReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantDetailReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<VariantDetailReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = 
                @$"
                    {BaseSqlQuery}
                    WHERE ""id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<VariantDetailReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<VariantDetailReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(VariantDetailReadModel), id);
    }
    
    public async Task<IReadOnlyList<VariantDetailReadModel>> FindByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""variant_id"" = @VariantId
                ");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantDetailReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            VariantId = variantId
                        },
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
}