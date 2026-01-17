using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Catalog.Domain.Entities;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ColorRepository : IColorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ColorSortBy, string> _sortColumnMapping = new()
    {
        { ColorSortBy.Id, "color_id" },
        { ColorSortBy.NormalizedName, "normalized_color_name" }
    };
    
    public ColorRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ColorRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<int> CreateAsync(Color color, CancellationToken cancellationToken)
    {
        var result = await this._context.Colors.AddAsync(color, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(Color color, CancellationToken cancellationToken)
    {
        var existingColor = await this.GetByIdAsync(color.Id, cancellationToken);
        
        _context.Colors.Update(color);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var color = await this.GetByIdAsync(id, cancellationToken);
        this._context.Colors.Remove(color);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Color>> FindAllAsync(
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
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
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "color_id");
        
            string sql =
                $@"
                    SELECT
                        ""color_id""              AS Id,
                        ""color_name""            AS Name,
                        ""normalized_color_name"" AS NormalizedName,
                        ""color_name_ru""         AS NameRu,
                        ""color_code""            AS ColorCode,
                        ""color_description""     AS Description
                    FROM
                        ""colors"" 
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<Color>(
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

    public async Task<Color?> FindByIdAsync(
        int id, 
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            string sql =  
                @"
                SELECT
                    ""color_id""              AS Id,
                    ""color_name""            AS Name,
                    ""normalized_color_name"" AS NormalizedName,
                    ""color_name_ru""         AS NameRu,
                    ""color_code""            AS ColorCode,
                    ""color_description""     AS Description
                FROM
                    ""colors""
                WHERE
                    ""color_id"" = @Id;
            ";
        
            return await connection
               .QueryFirstOrDefaultAsync<Color>(
                   sql, new { Id = id })
                       ?? null;
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<Color> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(Color), id);
    }
    
    public async Task<IEnumerable<Color?>> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "color_id");
        
            string sql = 
                $@"
                SELECT
                    ""color_id""              AS Id,
                    ""color_name""            AS Name,
                    ""normalized_color_name"" AS NormalizedName,
                    ""color_name_ru""         AS NameRu,
                    ""color_code""            AS ColorCode,
                    ""color_description""     AS Description
                FROM
                    ""colors""
                WHERE
                    ""normalized_color_name"" 
                        LIKE @Name
                ORDER BY {columnName} {direction} 
                LIMIT @Count
                OFFSET @Offset;
            ";
        
            return await connection
                .QueryAsync<Color>(
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

    public async Task<IEnumerable<Color?>> GetByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(Color), name);
        
        return result;
    }
}