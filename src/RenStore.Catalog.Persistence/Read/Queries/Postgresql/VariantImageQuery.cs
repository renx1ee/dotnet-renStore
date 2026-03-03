using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class VariantImageQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IVariantImageQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"                 AS Id,
                "original_file_name" AS OriginalFileName,
                "storage_path"       AS StoragePath,
                "file_size_bites"    AS FileSizeBytes,
                "is_main"            AS IsMain,
                "sort_order"         AS SortOrder,
                "uploaded_date"      AS UploadedAt,
                "updated_date"       AS UpdatedAt,
                "deleted_date"       AS DeletedAt,
                "weight"             AS Weight,
                "height"             AS Height,
                "is_deleted"         AS IsDeleted,
                "version"            AS Version,
                "variant_id"         AS VariantId
            FROM 
                "variant_images"
        """;

    private readonly Dictionary<VariantImageSortBy, string> _sortColumnMapping = new()
    {
        { VariantImageSortBy.Id, "id" },
        { VariantImageSortBy.UploadedAt, "uploaded_date" },
        { VariantImageSortBy.UpdatedAt, "updated_date" },
        { VariantImageSortBy.DeletedAt, "deleted_date" },
        { VariantImageSortBy.Version, "version" },
        { VariantImageSortBy.FileSizeBytes, "file_size_bites" },
    };

    public VariantImageQuery(
        ILogger<VariantImageQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<VariantImageReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantImageSortBy sortBy = VariantImageSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        bool? isMain = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");
            
            if (isDeleted.HasValue)
                sql.Append(" WHERE \"is_deleted\" = @IsDeleted");
            
            if (isMain.HasValue)
                sql.Append(" AND \"is_main\" = @IsMain");
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantImageReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            IsMain = isMain
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

    public async Task<VariantImageReadModel?> FindByIdAsync(
        CancellationToken cancellationToken,
        Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE ""id"" = @Id
                ";

            return await connection
                .QueryFirstOrDefaultAsync(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));
        }
        catch (Exception e)
        {
            throw Wrap(e);
        }
    }

    public async Task<VariantImageReadModel> GetByIdAsync(
        CancellationToken cancellationToken,
        Guid id)
    {
        return await this.FindByIdAsync(
                cancellationToken: cancellationToken, 
                id: id)
                ?? throw new NotFoundException(typeof(VariantImageReadModel), id);
    }
    
    public async Task<IReadOnlyList<VariantImageReadModel>> FindByVariantIdAsync(
        CancellationToken cancellationToken,
        Guid variantId,
        VariantImageSortBy sortBy = VariantImageSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        bool? isMain = null) 
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""variant_id"" = @VariantId
                ");

            if (isDeleted.HasValue)
                sql.Append(" AND \"is_deleted\" = @IsDeleted");
            
            if (isMain.HasValue)
                sql.Append(" AND \"is_main\" = @IsMain");
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantImageReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {   
                            VariantId = variantId,
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            IsMain = isMain
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