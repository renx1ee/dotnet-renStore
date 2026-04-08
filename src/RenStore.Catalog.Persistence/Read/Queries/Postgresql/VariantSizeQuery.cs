using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Contracts.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class VariantSizeQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IVariantSizeQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"           AS Id,
                "letter_size"  AS LetterSize,
                "size_number"  AS Number,
                "size_system"  AS System,
                "type"         AS Type,
                "is_deleted"   AS IsDeletedCategory,
                "created_date" AS CreatedAt,
                "updated_date" AS UpdatedAt,
                "deleted_date" AS DeletedAt,
                "variant_id"   AS VariantId
            FROM
                "variant_sizes"
        """;
    
    private readonly Dictionary<VariantSizeSortBy, string> _sortColumnMapping = new ()
    {
        { VariantSizeSortBy.Id,         "id" },
        { VariantSizeSortBy.LetterSize, "letter_size" },
        { VariantSizeSortBy.Number,     "size_number" },
        { VariantSizeSortBy.System,     "size_system" },
        { VariantSizeSortBy.Type,       "type" },
        { VariantSizeSortBy.CreatedAt,  "created_date" },
        { VariantSizeSortBy.UpdatedAt,  "updated_date" },
        { VariantSizeSortBy.DeletedAt,  "deleted_date" },
    };
    
    public VariantSizeQuery(
        ILogger<VariantSizeQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }
    
    public async Task<IReadOnlyList<VariantSizeReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
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
                @$"
                    {BaseSqlQuery}
                ");
            
            if (isDeleted.HasValue)
                sql.Append(" WHERE \"is_deleted\" = @IsDeletedCategory");
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantSizeReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
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

    public async Task<VariantSizeReadModel?> FindByIdAsync(
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
                    WHERE "id" @Id;
                """;

            return await connection
                .QueryFirstOrDefaultAsync<VariantSizeReadModel>(
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

    public async Task<VariantSizeReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(VariantSizeReadModel), id);
    }
    
    public async Task<IReadOnlyList<VariantSizeReadModel>> FindByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken,
        VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
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
                $"""
                    {BaseSqlQuery}
                    WHERE "variant_id" = @VariantId
                """);
            
            if (isDeleted.HasValue)
                sql.Append(" AND \"is_deleted\" = @IsDeletedCategory");
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<VariantSizeReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            VariantId = variantId
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