using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class PickupPointQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.IPickupPointQuery
{
    private const string BaseSqlQuery =
        """ 
            SELECT
                ""pickup_point_id"" AS Id,
                ""code""            AS Code,
                ""is_deleted""      AS IsDeleted,
                ""address_id""      AS AddressId,
                ""created_date""    AS CreatedAt,
                ""delete_date""     AS DeletedAt
            FROM
                ""pickup_points""
        """;
    
    private readonly Dictionary<PickupSortBy, string> _sortColumnMapping = new()
    {
        { PickupSortBy.Id, "pickup_point_id" },
        { PickupSortBy.Code, "code" },
        { PickupSortBy.AddressId, "address_id" },
        { PickupSortBy.CreatedAt, "created_date" },
        { PickupSortBy.DeletedAt, "delete_date" }
    };
    
    public PickupPointQuery(
        ILogger<PickupPointQuery> logger,
        ApplicationDbContext context) 
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<PickupPointReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
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
                .QueryAsync<PickupPointReadModel>(
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

    public async Task<PickupPointReadModel?> FindByIdAsync(
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
                .QueryFirstOrDefaultAsync<PickupPointReadModel>(
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

    public async Task<PickupPointReadModel> GetByIdAsync(
        long id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(PickupPointReadModel), id);
    }

    public async Task<IReadOnlyList<PickupPointReadModel>> FindByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
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
                .QueryAsync<PickupPointReadModel>(
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

    public async Task<IReadOnlyList<PickupPointReadModel>> GetByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
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
            throw new NotFoundException(typeof(PickupPointReadModel), addressId);

        return result;
    }
}