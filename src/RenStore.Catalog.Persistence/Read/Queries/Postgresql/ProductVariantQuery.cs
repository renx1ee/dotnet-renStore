using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Contracts.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class ProductVariantQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IProductVariantQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"              AS Id,
                "name"            AS Name,
                "normalized_name" AS NormalizedName,
                "article"         AS Article,
                "status"          AS Status,
                "url"             AS Url,
                "created_date"    AS CreatedAt,
                "updated_date"    AS UpdatedAt,
                "deleted_date"    AS DeletedAt,
                "product_id"      AS ProductId,
                "color_id"        AS ColorId,
                "main_image_id"   AS MainImageId,
                "size_system"     AS SizeSystem,
                "size_type"       AS SizeType
            FROM
                "product_variants"
        """;

    private readonly Dictionary<ProductVariantSortBy, string> _sortColumnMapping = new()
    {
        { ProductVariantSortBy.Id, "id" },
        { ProductVariantSortBy.Name, "name" },
        { ProductVariantSortBy.Article, "article" },
        { ProductVariantSortBy.Status, "status" },
        { ProductVariantSortBy.CreatedAt, "created_date" },
        { ProductVariantSortBy.UpdatedAt, "updated_date" },
        { ProductVariantSortBy.DeletedAt, "deleted_date" },
        { ProductVariantSortBy.SizeSystem, "size_systemd" },
        { ProductVariantSortBy.SizeType, "size_type" },
    };

    public ProductVariantQuery(
        ILogger<ProductVariantQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<ProductVariantReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(page, pageSize, descending);

            var sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append(" WHERE \"status\" = \"is_deleted\"");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");
            
            var result = await connection
                .QueryAsync<ProductVariantReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
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

    public async Task<ProductVariantReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql =
                $@"
                    {BaseSqlQuery}
                    WHERE ""id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductVariantReadModel>(
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

    public async Task<ProductVariantReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(ProductVariantReadModel), id);
    }
    
    public async Task<ProductVariantReadModel?> FindByArticleAsync(
        long article,
        CancellationToken cancellationToken)
    {
        if (article <= 0)
            throw new ArgumentOutOfRangeException(nameof(article));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql =
                $@"
                    {BaseSqlQuery}
                    WHERE ""article"" = @Article
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductVariantReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Article = article },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<ProductVariantReadModel>> FindByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);

            var sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                    WHERE ""product_id"" = @ProductId
                ");

            if (isDeleted.HasValue)
                sql.Append(" AND \"status\" = \"is_deleted\"");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<ProductVariantReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            ProductId = productId
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
    
    // TODO: search by name
}