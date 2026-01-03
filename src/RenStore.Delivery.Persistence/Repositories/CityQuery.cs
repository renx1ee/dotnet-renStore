using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Repositories;

public class CityQuery(
    ILogger<CityQuery> logger,
    ApplicationDbContext context)
{
    private const int CommandTimeoutSeconds = 30;
    private const uint MaxPageSize = 1000;
    
    private const string BaseSqlQuery = 
        """ 
            SELECT
            "city_id"                 AS Id,
            "city_name"               AS Name,
            "normalized_city_name"    AS NormalizedName,
            "city_name_ru"            AS NameRu,
            "normalized_city_name_ru" AS NormalizedNameRu,
            "country_id"              AS CountryOfManufactureId
        FROM
            "cities"
        """;
    
    private readonly Dictionary<CitySortBy, string> _sortColumnMapping = new ()
    {
        { CitySortBy.Id, "city_id" },
        { CitySortBy.Name, "city_name" }
    };
    
    private readonly ILogger<CityQuery> _logger    = logger 
                                                     ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));
    
    private DbTransaction? CurrentTransaction =>
        this._context.Database.CurrentTransaction?.GetDbTransaction();
    
    public async Task<IReadOnlyList<City>> FindAllAsync(
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildCityRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            string sql =
                $@"
                    {BaseSqlQuery}
                    ORDER BY 
                        {columnName} {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<City>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            Count = (int)pageRequest.Limit,
                            Offset = (int)pageRequest.Offset
                        },
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken,
                        transaction: CurrentTransaction));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }

    public async Task<City?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot less 1.");
        
        try
        {
            var connection = await GetOpenConnectionAsync(cancellationToken);

            const string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""city_id"" = @Id;
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<City>(
                    new CommandDefinition(
                        commandText: sql, 
                        parameters: new 
                            { Id = id },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));   
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<City> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(City), id);
    }

    public async Task<IReadOnlyList<City>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            var connection = await GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildCityRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);

            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE
                        ""normalized_city_name"" 
                            LIKE @Name
                    ORDER BY 
                        {columnName} {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<City>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            Name = $"%{name.ToUpper()}%",
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentTransaction,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IEnumerable<City>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageSize, page, descending);
        
        if (result.Count == 0)
            throw new NotFoundException(typeof(ColorEntity), name);
        
        return result;
    }
    
    public async Task<IReadOnlyList<City>> FindByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        if (countryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(countryId));
        
        try
        {
            var connection = await GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildCityRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);

            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE
                        ""country_id"" = @CountryId
                    ORDER BY 
                        {columnName} {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<City>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            CountryId = countryId,
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentTransaction,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<City>> GetByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByCountryIdAsync(
            countryId: countryId,
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageSize: pageSize,
            page: page,
            descending: descending);

        if (result.Count == 0)
            throw new NotFoundException(typeof(City), countryId);

        return result;
    }

    private async Task<DbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = this._context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }
    private static CityPageRequest BuildCityRequest(uint page, uint pageSize, bool descending)
    {
        pageSize = Math.Min(pageSize, MaxPageSize);
        uint offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";
        
        return new CityPageRequest(
            Limit: pageSize,
            Offset: offset,
            Direction: direction);
    }

    private DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occured", e);
    }
}

readonly record struct CityPageRequest(uint Limit, uint Offset, string Direction);