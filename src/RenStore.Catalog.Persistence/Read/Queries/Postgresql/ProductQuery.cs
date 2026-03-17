using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class ProductQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      RenStore.Catalog.Application.Abstractions.Queries.IProductQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"              AS Id,
                "status"          AS Status,
                "created_date"    AS CreatedAt,
                "updated_date"    AS UpdatedAt,
                "deleted_date"    AS DeletedAt,
                "seller_id"       AS SellerId,
                "sub_category_id" AS SubCategoryId
            FROM
                "products"
        """;
    
    private readonly Dictionary<ProductSortBy, string> _sortColumnMapping = new ()
    {
        { ProductSortBy.Id,        "id" },
        { ProductSortBy.Status,    "status" },
        { ProductSortBy.CreatedAt, "created_date" },
        { ProductSortBy.UpdatedAt, "updated_date" },
        { ProductSortBy.DeletedAt, "deleted_date" }
    };

    public ProductQuery(
        ILogger<ProductQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<ProductReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null)
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

            if (isDeleted.HasValue)
                sql.Append(" WHERE \"status\" = is_deleted");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<ProductReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            IsDeleted = isDeleted,
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit
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

    public async Task<ProductReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
            
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql =
                $"""
                    {BaseSqlQuery}
                    WHERE "id" = @Id;
                """;

            return await connection
                .QueryFirstOrDefaultAsync<ProductReadModel>(
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

    public async Task<ProductReadModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(ProductReadModel), id);
    }
        
    public async Task<IReadOnlyList<ProductReadModel>> FindBySellerIdAsync(
        Guid sellerId,
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (sellerId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sellerId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""seller_id"" = @SellerId
                ");

            if (isDeleted.HasValue)
            {
                if (isDeleted == true)
                {
                    var deleteColumn = ProductStatusConversion.ToDatabase(
                        ProductStatus.Deleted);
                
                    sql.Append($""" AND "status" = '{deleteColumn}' """);
                }
                else
                {
                    var deleteColumn = ProductStatusConversion.ToDatabase(
                        ProductStatus.Deleted);
                
                    sql.Append($""" AND "status" != '{deleteColumn}' """);
                }
            }

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<ProductReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            IsDeleted = isDeleted,
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
                            SellerId = sellerId
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
    
    public async Task<IReadOnlyList<ProductReadModel>> FindBySubCategoryIdAsync(
        Guid subCategoryId,
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (subCategoryId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sortBy));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""sub_category_id"" = @SubCategoryId
                ");

            if (isDeleted.HasValue)
                sql.Append(" AND \"status\" = \"is_deleted\"");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<ProductReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            IsDeleted = isDeleted,
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
                            SubCategoryId = subCategoryId 
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