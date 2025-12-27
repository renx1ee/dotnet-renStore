using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<CitySortBy, string> _sortColumnMapping = new ()
        {
            { CitySortBy.Id, "city_id" },
            { CitySortBy.Name, "city_name" }
        };

    public CityRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
            ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public CityRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<int> CreateAsync(
        CityEntity city, 
        CancellationToken cancellationToken)
    {
        var result = await _context.Cities.AddAsync(city, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(
        CityEntity city, 
        CancellationToken cancellationToken)
    {
        var cityExists = await this.GetByIdAsync(city.Id, cancellationToken);
        _context.Cities.Update(city);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(
        int id, 
        CancellationToken cancellationToken)
    {
        var city = await this.GetByIdAsync(id, cancellationToken);
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CityEntity?>> FindAllAsync(
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "city_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                $@"
                    SELECT
                        ""city_id""                 AS Id,
                        ""city_name""               AS Name,
                        ""normalized_city_name""    AS NormalizedName,
                        ""city_name_ru""            AS NameRu,
                        ""normalized_city_name_ru"" AS NormalizedNameRu,
                        ""country_id""              AS CountryOfManufactureId
                    FROM
                        ""cities""
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<CityEntity?>(
                    sql, new
                    {
                        Count = (int)pageCount,
                        Offset = (int)offset,
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }

    public async Task<CityEntity?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT
                        ""city_id""                 AS Id,
                        ""city_name""               AS Name,
                        ""normalized_city_name""    AS NormalizedName,
                        ""city_name_ru""            AS NameRu,
                        ""normalized_city_name_ru"" AS NormalizedNameRu,
                        ""country_id""              AS CountryOfManufactureId
                    FROM
                        ""cities""
                    WHERE
                        ""city_id"" = @Id;
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<CityEntity?>(
                    sql, new { Id = id });   
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<CityEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(CityEntity), id);
    }

    public async Task<IEnumerable<CityEntity?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);
            
            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "city_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""city_id""                 AS Id,
                        ""city_name""               AS Name,
                        ""normalized_city_name""    AS NormalizedName,
                        ""city_name_ru""            AS NameRu,
                        ""normalized_city_name_ru"" AS NormalizedNameRu,
                        ""country_id""              AS CountryOfManufactureId
                    FROM
                        ""cities""
                    WHERE
                        ""normalized_city_name"" 
                            LIKE @Name
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";
            
            return await connection
                .QueryAsync<CityEntity?>(
                    sql, new
                    {
                        Name = $"%{name.ToUpper()}%",
                        Count = (int)pageCount,
                        Offset = (int)offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<CityEntity?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(ColorEntity), name);
        
        return result;
    }
}