using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class CityQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.ICityQuery
{
    private const string BaseSqlQuery = 
        """ 
            SELECT
                "city_id"                 AS Id,
                "city_name"               AS Name,
                "normalized_city_name"    AS NormalizedName,
                "city_name_ru"            AS NameRu,
                "normalized_city_name_ru" AS NormalizedNameRu,
                "is_deleted"              AS IsDeleted,
                ""created_date""          AS CreatedAt,
                ""updated_date""          AS UpdatedAt,
                ""deleted_date""          AS DeletedAt,
                "country_id"              AS CountryOfManufactureId
            FROM
                "cities"
        """;
    
    private readonly Dictionary<CitySortBy, string> _sortColumnMapping = new ()
    {
        { CitySortBy.Id, "city_id" },
        { CitySortBy.Name, "city_name" },
        { CitySortBy.NameRu, "city_name_ru" },
        { CitySortBy.CountryId, "country_id" }
    };
    
    public CityQuery(
        ILogger<CityQuery> logger,
        DeliveryDbContext context) 
        : base(context, logger)
    {
    }
    
    public async Task<IReadOnlyList<CityReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append($" WHERE \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CityReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = (int)pageRequest.Limit,
                            Offset = (int)pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken,
                        transaction: CurrentDbTransaction));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }

    public async Task<CityReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot less 1.");
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            string sql = 
                @$"
                    {BaseSqlQuery}
                    WHERE ""city_id"" = @Id
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<CityReadModel>(
                    new CommandDefinition(
                        commandText: sql, 
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
    
    public async Task<CityReadModel> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(CityReadModel), id);
    }

    public async Task<IReadOnlyList<CityReadModel>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE
                        ""normalized_city_name"" 
                            LIKE @Name
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CityReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Name = $"%{name.ToUpper()}%",
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
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

    public async Task<IReadOnlyList<CityReadModel>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByNameAsync(
            name: name, 
            cancellationToken: cancellationToken, 
            sortBy: sortBy, 
            pageSize: pageSize, 
            page: page, 
            descending: descending, 
            isDeleted: isDeleted);
        
        if (result.Count == 0)
            throw new NotFoundException(typeof(CityReadModel), name);
        
        return result;
    }
    
    public async Task<IReadOnlyList<CityReadModel>> FindByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (countryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(countryId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    WHERE ""country_id"" = @CountryId
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CityReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            CountryId = countryId,
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
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

    public async Task<IReadOnlyList<CityReadModel>> GetByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByCountryIdAsync(
            countryId: countryId,
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageSize: pageSize,
            page: page,
            descending: descending,
            isDeleted: isDeleted);

        if (result.Count == 0)
            throw new NotFoundException(typeof(CityReadModel), countryId);

        return result;
    }
}