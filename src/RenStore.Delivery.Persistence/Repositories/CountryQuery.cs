using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Repositories;

public class CountryQuery
    (ILogger<CountryQuery> logger,
    ApplicationDbContext context)
    : RenStore.Delivery.Application.Interfaces.ICountryQuery
{
    private const uint MaxPageSize = 1000;
    private const int CommandTimeoutSeconds = 30;

    private const string BaseSqlQuery =
        """
            SELECT
                ""country_id""                 AS Id,
                ""country_name""               AS Name,
                ""normalized_country_name""    AS NormalizedName,
                ""country_name_ru""            AS NameRu,
                ""normalized_country_name_ru"" AS NormalizedNameRu,
                ""country_code""               AS Code,
                ""country_phone_code""         AS Code
            FROM
                ""countries"" AS c
        """;
    
    private readonly Dictionary<CountrySortBy, string> _sortColumnMapping = new ()
    {
        { CountrySortBy.Id, "country_id" },
        { CountrySortBy.Name, "country_name" }
    };
    
    private readonly ILogger<CountryQuery> _logger = logger 
                                                     ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));

    private DbTransaction? CurrentTransaction =>
        this._context.Database.CurrentTransaction?.GetDbTransaction();
    
    public async Task<IReadOnlyList<Country>> FindAllAsync(
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                pageSize: pageSize, 
                page: page, 
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
                .QueryAsync<Country>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
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

    public async Task<Country?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id == 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            const string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""country_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<Country>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<Country?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(Country), id);
    }

    public async Task<IReadOnlyList<Country>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);

            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE
                        ""normalized_country_name"" 
                            LIKE @Name
                    OR 
                        ""normalized_other_name""
                            LIKE @Name
                    OR 
                        ""normalized_country_name_ru""
                            LIKE @Name
                    ORDER BY 
                        {columnName} {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<Country>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            Name = $"%{name.ToUpper()}%",
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

    public async Task<IEnumerable<Country>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageSize, page, descending);
        
        if (result.Count == 0) throw new NotFoundException(typeof(Country), name);

        return result;
    }
    
    public async Task<IReadOnlyList<Country>> FindByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        if(cityId <= 0)
            throw new ArgumentException(nameof(cityId));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);

            string sql =
                @$"
                    {BaseSqlQuery}
                    JOIN ""cities"" AS ci
                        ON c.""country_id"" = ci.""country_id""
                    WHERE 
                        ci.""city_id"" = @CityId
                    ORDER BY 
                        {columnName} {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<Country>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        {
                            CityId = cityId,
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

    public async Task<IReadOnlyList<Country>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByCityIdAsync(cityId, cancellationToken, sortBy, pageSize, page, descending);
        
        if (result.Count == 0) throw new NotFoundException(typeof(Country), cityId);

        return result;
    }
    
    private async Task<DbConnection> GetOpenConnectionAsync(
        CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }

    private static CountryPageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
    {
        if (page == 0)
            throw new ArgumentOutOfRangeException();
        
        pageSize = Math.Min(pageSize, MaxPageSize);
        uint offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";

        return new CountryPageRequest(
            Limit: (int)pageSize,
            Offset: (int)offset,
            Direction: direction);
    }

    private DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occured.", e);
    }
}

readonly record struct CountryPageRequest(int Limit, int Offset, string Direction);