using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Inventory.Application.ReadModels;
using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.Persistence.Read.Queries;

internal sealed class StockQuery(InventoryDbContext context, ILogger<StockQuery> logger) 
    : RenStore.Inventory.Persistence.Read.Base.DapperQueryBase(context, logger),
      RenStore.Inventory.Application.Abstractions.Queries.IStockQuery
{
    private static readonly Dictionary<StockSortBy, string> _stockSortColumnMapping = new()
    {
        { StockSortBy.Id,             "id" },
        { StockSortBy.InStock,        "in_stock" },
        { StockSortBy.Sales,          "sales" },
        { StockSortBy.WriteOffReason, "write_off_reason" },
        { StockSortBy.CreatedAt,      "created_date" },
        { StockSortBy.UpdatedAt,      "updated_date" },
        { StockSortBy.DeletedAt,      "deleted_date" },
        { StockSortBy.VariantId,      "variant_id" },
        { StockSortBy.SizeId,         "size_id" },
    };
    
    public async Task<VariantStockDto?> FindByIdAsync(
        Guid id,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                {GetBaseSql()}
                WHERE "id" = @Id
                """);
            
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Id", id);

            if (isDeleted.HasValue)
            {
                sql.Append(""" AND "is_deleted" = @IsDeleted """);
                dynamicParameters.Add("IsDeleted", isDeleted.Value);
            }

            return await connection
                .QueryFirstOrDefaultAsync<VariantStockDto>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: dynamicParameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<VariantStockDto>> FindByVariantIdAsync(
        Guid variantId,
        StockSortBy sortBy = StockSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));

        if (!_stockSortColumnMapping.TryGetValue(sortBy, out var columnMapping))
            throw new ArgumentOutOfRangeException(nameof(sortBy));

        var pageRequest = BuildPageRequest(
            page: page,
            pageSize: pageSize,
            descending: descending);
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                 {GetBaseSql()}
                 WHERE "variant_id" = @VariantId    
                 """);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("VariantId", variantId);
            dynamicParameters.Add("Count", pageRequest.Limit);
            dynamicParameters.Add("Offset", pageRequest.Offset);
            
            if (isDeleted.HasValue)
            {
                sql.Append(""" AND "is_deleted" = @IsDeleted """);
                dynamicParameters.Add("IsDeleted", isDeleted.Value);
            }
            
            sql.Append(
                $""" 
                ORDER BY {columnMapping} {pageRequest.Direction}
                LIMIT @Count
                OFFSET @Offset;
                """);

            var result =  await connection
                .QueryAsync<VariantStockDto>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: dynamicParameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<VariantStockDto?> FindByVariantIdAsync(
        Guid variantId,
        Guid sizeId,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
        
        if (sizeId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sizeId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = new StringBuilder(
                $"""
                 {GetBaseSql()}
                 WHERE "variant_id" = @VariantId
                 AND "size_id"      = @SizeId
                 """);
            
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("VariantId", variantId);
            dynamicParameters.Add("SizeId", sizeId);
            
            if (isDeleted.HasValue)
            {
                sql.Append(""" AND "is_deleted" = @IsDeleted """);
                dynamicParameters.Add("IsDeleted", isDeleted.Value);
            }

            return await connection
                .QueryFirstOrDefaultAsync<VariantStockDto>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: dynamicParameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    private string GetBaseSql()
    {
        return """
               SELECT 
                   "id"               AS Id,
                   "in_stock"         AS InStock,
                   "sales"            AS Sales,
                   "write_off_reason" AS WriteOffReason,
                   "created_date"     AS CreatedAt,
                   "updated_date"     AS UpdatedAt,
                   "deleted_date"     AS DeletedAt,
                   "is_deleted"       AS IsDeleted,
                   "variant_id"       AS VariantId,
                   "size_id"          AS SizeId
               FROM "stocks"
               """;
    }
}