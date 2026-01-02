using System.Data;
using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Features.Address.Queries;
using RenStore.Delivery.Application.Interfaces;
using RenStore.Delivery.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Repositories;

internal sealed class AddressQuery(
    ILogger<AddressRepository> logger,
    ApplicationDbContext context) 
    : IAddressQuery
{
    private const uint MaxPageSize = 1000;
    private const int CommandTimeoutSeconds = 30;
    
    private const string BaseSqlQuery = 
        """
            SELECT
                ""address_id""       AS Id,
                ""house_code""       AS HouseCode,
                ""street""           AS Street,
                ""building_number""  AS BuildingNumber,
                ""apartment_number"" AS ApartmentNumber,
                ""entrance""         AS Entrance,
                ""floor""            AS Floor,
                ""flat_number""      AS FlatNumber,
                ""full_address""     AS FullAddress,
                ""created_date""     AS CreatedAt,
                ""updated_date""     AS UpdatedAt,
                ""is_deleted""       AS IsDeleted,
                ""user_id""          AS ApplicationUserId,
                ""country_id""       AS CountryId,
                ""city_id""          AS CityId
            FROM
                "addresses"
        """;
    
    private static readonly Dictionary<AddressSortBy, string> _sortColumnMapping = new()
    {
        { AddressSortBy.Id, "address_id" },
        { AddressSortBy.HouseCode, "house_code" },
        { AddressSortBy.FlatNumber, "flat_number" }
    };
    
    private readonly ILogger<AddressRepository> _logger = logger 
                                                          ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context      = context 
                                                          ?? throw new ArgumentNullException(nameof(context));
    
    private DbTransaction? CurrentTransaction => 
        this._context.Database.CurrentTransaction?.GetDbTransaction();
    
    public async Task<IReadOnlyList<AddressReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false)
    {
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            string sql = 
                @$"
                    {BaseSqlQuery}
                    ORDER BY ""{columnName}"" {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<AddressReadModel>(
                    new CommandDefinition(
                        commandText: sql, 
                        parameters: new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<AddressReadModel>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        bool? includeDeleted = null,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var sql = new StringBuilder(
                $@" 
                    {BaseSqlQuery}
                    WHERE 1 = 1
                ");

            var parameters = new DynamicParameters();
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (includeDeleted.HasValue)
            {
                sql.Append(" AND \"is_deleted\" = @IsDeleted");
                parameters.Add("IsDeleted", includeDeleted.Value);
            }
            
            if (criteria.CountryId.HasValue)
            {
                sql.Append(" AND \"country_id\" = @CountryId");
                parameters.Add("CountryId", criteria.CountryId);
            }

            if (criteria.CityId.HasValue)
            {
                sql.Append(" AND \"city_id\" = @CityId");
                parameters.Add("CityId", criteria.CityId); 
            }
            
            if (!string.IsNullOrEmpty(criteria.UserId))
            {
                sql.Append(" AND \"user_id\" = @UserId");
                parameters.Add("UserId", criteria.UserId); 
            }
            
            if (!string.IsNullOrEmpty(criteria.Street))
            {
                sql.Append(" AND \"street\" = @Street");
                parameters.Add("Street", criteria.Street); 
            }
            
            if (!string.IsNullOrEmpty(criteria.BuildingNumber))
            {
                sql.Append(" AND \"building_number\" = @BuildingNumber");
                parameters.Add("BuildingNumber", criteria.BuildingNumber); 
            }
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction} 
                           LIMIT @Count OFFSET @Offset;");

            var result = await connection
                .QueryAsync<AddressReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: parameters,
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<AddressReadModel?> FindByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""address_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<AddressReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<AddressReadModel> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(AddressReadModel), id);
    }
    
    public async Task<IReadOnlyList<AddressReadModel>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("UserId cannot be empty", nameof(userId));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
            {
                _logger.LogWarning("{Argument} out of range.", sortBy);
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            }
            
            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE 
                        ""user_id"" = @UserId
                    ORDER BY ""{columnName}"" {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<AddressReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters:new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            UserId = userId
                        },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<bool> IsExists(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("AddressId cannot be empty!", nameof(id));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);
            
            string sql = 
                @"
                    SELECT EXISTS (
                        SELECT 1
                        FROM 
                            ""addresses""
                        WHERE 
                            ""address_id"" = @Id
                    );
                ";

            var result = await connection
                .ExecuteScalarAsync<bool>(
                    new CommandDefinition(
                        parameters: new { Id = id},
                        commandText: sql,
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result;
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    private async Task<DbConnection> GetOpenConnectionAsync(
        CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
            
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }

    private DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occured.", e);
    }
    
    private static PageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
    {
        if (page == 0) 
            throw new ArgumentOutOfRangeException(nameof(page));
        
        pageSize = Math.Min(pageSize, MaxPageSize);
        uint offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";

        return new PageRequest(
            Limit: (int)pageSize,
            Offset: (int)offset,
            Direction: direction);
    }
}

readonly record struct PageRequest(int Limit, int Offset, string Direction);