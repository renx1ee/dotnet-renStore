using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<CountrySortBy, string> _sortColumnMapping = new ()
    {
        { CountrySortBy.Id, "country_id" },
        { CountrySortBy.Name, "country_name" }
    };

    public CountryRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public CountryRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<int> CreateAsync(CountryEntity country, CancellationToken cancellationToken)
    {
        var result = await _context.Countries.AddAsync(country, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(CountryEntity country, CancellationToken cancellationToken)
    {
        var countryExists = await this.GetByIdAsync(country.Id, cancellationToken);
        _context.Countries.Update(country);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var country = await this.GetByIdAsync(id, cancellationToken);
        _context.Countries.Remove(country);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CountryEntity?>> FindAllAsync(
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "country_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                $@"
                    SELECT
                        ""country_id""                 AS Id,
                        ""country_name""               AS Name,
                        ""normalized_country_name""    AS NormalizedName,
                        ""other_name""                 AS OtherName,
                        ""normalized_other_name""      AS NormalizedOtherName,
                        ""country_name_ru""            AS NameRu,
                        ""normalized_country_name_ru"" AS NormalizedNameRu,
                        ""country_code""               AS Code,
                        ""country_phone_code""         AS Code
                    FROM
                        ""countries""
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<CountryEntity?>(
                    sql, new
                    {
                        Count = (int)pageCount,
                        Offset = (int)offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }

    public async Task<CountryEntity?> FindByIdAsync(
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
                        ""country_id""                 AS Id,
                        ""country_name""               AS Name,
                        ""normalized_country_name""    AS NormalizedName,
                        ""other_name""                 AS OtherName,
                        ""normalized_other_name""      AS NormalizedOtherName,
                        ""country_name_ru""            AS NameRu,
                        ""normalized_country_name_ru"" AS NormalizedNameRu,
                        ""country_code""               AS Code,
                        ""country_phone_code""         AS Code
                    FROM
                        ""countries""
                    WHERE
                        ""country_id"" = @Id;
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<CountryEntity>(
                    sql, new { Id = id });   
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<CountryEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(CountryEntity), id);
    }

    public async Task<IEnumerable<CountryEntity?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Name,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);
            
            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "country_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""country_id""                 AS Id,
                        ""country_name""               AS Name,
                        ""normalized_country_name""    AS NormalizedName,
                        ""other_name""                 AS OtherName,
                        ""normalized_other_name""      AS NormalizedOtherName,
                        ""country_name_ru""            AS NameRu,
                        ""normalized_country_name_ru"" AS NormalizedNameRu,
                        ""country_code""               AS Code,
                        ""country_phone_code""         AS Code
                    FROM
                        ""countries""
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
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";
            
            return await connection
                .QueryAsync<CountryEntity?>(
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

    public async Task<IEnumerable<CountryEntity?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Name,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending);
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(CountryEntity), name);

        return result;
    }
}