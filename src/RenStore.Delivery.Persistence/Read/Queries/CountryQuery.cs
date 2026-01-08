using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class CountryQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.ICountryQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                ""country_id""                 AS Id,
                ""country_name""               AS Name,
                ""normalized_country_name""    AS NormalizedName,
                ""country_name_ru""            AS NameRu,
                ""normalized_country_name_ru"" AS NormalizedNameRu,
                ""country_code""               AS Code,
                ""country_phone_code""         AS Code,
                ""is_deleted""                 AS IsDeleted,
                ""created_date""               AS CreatedAt,
                ""updated_date""               AS UpdatedAt,
                ""deleted_date""               AS DeletedAt
            FROM
                ""countries"" AS c
        """;
    
    private readonly Dictionary<CountrySortBy, string> _sortColumnMapping = new ()
    {
        { CountrySortBy.Id, "country_id" },
        { CountrySortBy.Name, "country_name" },
        { CountrySortBy.NameRu, "country_name_ru" },
        { CountrySortBy.Code, "country_code" },
        { CountrySortBy.PhoneCode, "country_phone_code" }
    };
    
    public CountryQuery(
        ILogger<CountryQuery> logger,
        DeliveryDbContext context) 
        : base(context, logger)
    {
    }
    
    public async Task<IReadOnlyList<CountryReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                pageSize: pageSize, 
                page: page, 
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
                .QueryAsync<CountryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
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

    public async Task<CountryReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id == 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            const string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""country_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<CountryReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new
                        { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<CountryReadModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(CountryReadModel), id);
    }

    public async Task<IReadOnlyList<CountryReadModel>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

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
                        ""normalized_country_name"" 
                            LIKE @Name
                    OR 
                        ""normalized_other_name""
                            LIKE @Name
                    OR 
                        ""normalized_country_name_ru""
                            LIKE @Name
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CountryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Name = $"%{name.ToUpper()}%",
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

    public async Task<IEnumerable<CountryReadModel>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
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
        
        if (result.Count == 0) throw new NotFoundException(typeof(CountryReadModel), name);

        return result;
    }
    
    public async Task<IReadOnlyList<CountryReadModel>> FindByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        if(cityId <= 0)
            throw new ArgumentException(nameof(cityId));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                    JOIN ""cities"" AS ci
                        ON c.""country_id"" = ci.""country_id""
                    WHERE 
                        ci.""city_id"" = @CityId
                ");

            if (isDeleted.HasValue)
                sql.Append($" AND c.\"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset;");

            var result = await connection
                .QueryAsync<CountryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            CityId = cityId,
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

    public async Task<IReadOnlyList<CountryReadModel>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null)
    {
        var result = await this.FindByCityIdAsync(
            cityId: cityId, 
            cancellationToken: cancellationToken, 
            sortBy: sortBy, 
            pageSize: pageSize, 
            page: page, 
            descending: descending,
            isDeleted: isDeleted);
        
        if (result.Count == 0) throw new NotFoundException(typeof(CountryReadModel), cityId);

        return result;
    }
}