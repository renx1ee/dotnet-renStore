using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class SortingCenterQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.ISortingCenterQuery
{
    private const string BaseSqlQuery =
        """ 
            SELECT
                ""sorting_center_id"" AS Id,
                ""code""              AS Code,
                ""is_deleted""        AS IsDeleted,
                ""address_id""        AS AddressId,
                ""created_date""      AS CreatedAt,
                ""delete_date""       AS DeletedAt
            FROM
                ""sorting_centers""
        """;
    
    private readonly Dictionary<SortingCenterSortBy, string> _sortColumnMapping = new()
    {
        { SortingCenterSortBy.Id, "sorting_center_id" },
        { SortingCenterSortBy.Code, "code" },
        { SortingCenterSortBy.AddressId, "address_id" },
        { SortingCenterSortBy.CreatedAt, "created_date" },
        { SortingCenterSortBy.DeletedAt, "delete_date" }
    };
    
    public SortingCenterQuery(
        ILogger<SortingCenterQuery> logger,
        ApplicationDbContext context)
        : base(context, logger)
    {
    }
    
    public async Task<IReadOnlyList<SortingCenterReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page, 
                pageSize: pageSize, 
                descending: descending);

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append(@" WHERE ""is_deleted"" = @IsDeleted");
            
            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result =  await connection
                .QueryAsync<SortingCenterReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
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
    
    public async Task<SortingCenterReadModel?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            string sql = "";

            return await connection
                .QueryFirstOrDefaultAsync<SortingCenterReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new { Id = id },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<SortingCenterReadModel> GetByIdAsync(
        long id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(SortingCenterReadModel), id);
    }
    
    public async Task<IReadOnlyList<SortingCenterReadModel>> FindByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (addressId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(addressId));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);
            
            var pageRequest = BuildPageRequest(
                page: page, 
                pageSize: pageSize, 
                descending: descending);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""address_id"" = @Id
                ");

            if (isDeleted.HasValue)
                sql.Append(@" AND ""is_deleted"" = @IsDeleted");
            
            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result =  await connection
                .QueryAsync<SortingCenterReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Id = addressId,
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
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

    public async Task<IReadOnlyList<SortingCenterReadModel>> GetByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByAddressIdAsync(
            addressId: addressId,
            cancellationToken: cancellationToken,
            page: page,
            pageSize: pageSize,
            sortBy: sortBy,
            descending: descending,
            isDeleted: isDeleted);

        if (result.Count == 0)
            throw new NotFoundException(typeof(SortingCenterReadModel), addressId);

        return result;
    }
}