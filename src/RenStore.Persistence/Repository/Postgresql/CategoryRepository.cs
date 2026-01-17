using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<CategorySortBy, string> _sortColumnMapping = new()
    {
        { CategorySortBy.Id, "category_id" },
        { CategorySortBy.Name, "normalized_category_name" },
        { CategorySortBy.NameRu, "normalized_category_name_ru" },
    };
    
    public CategoryRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public CategoryRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<int> CreateAsync(Category category, CancellationToken cancellationToken)
    {
        var result = await this._context.Categories.AddAsync(category, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        var existingCategory = await this.GetByIdAsync(category.Id, cancellationToken);
        
        _context.Categories.Update(category);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await this.GetByIdAsync(id, cancellationToken);
        this._context.Categories.Remove(category);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Category>> FindAllAsync(
        CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "category_id");
        
            string sql =
                $@"
                    SELECT
                        ""category_id""                 AS Id,
                        ""category_name""               AS Name,
                        ""normalized_category_name""    AS NormalizedName,
                        ""category_name_ru""            AS NameRu,
                        ""normalized_category_name_ru"" AS NormalizedNameRu,
                        ""category_description""        AS Description,
                        ""is_active""                   AS IsActive,
                        ""created_date""                AS OccuredAt
                    FROM
                        ""categories"" 
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<Category>(
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
    
    public async Task<Category?> FindByIdAsync(
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
                        ""category_id""                 AS Id,
                        ""category_name""               AS Name,
                        ""normalized_category_name""    AS NormalizedName,
                        ""category_name_ru""            AS NameRu,
                        ""normalized_category_name_ru"" AS NormalizedNameRu,
                        ""category_description""        AS Description,
                        ""is_active""                   AS IsActive,
                        ""created_date""                AS OccuredAt
                    FROM
                        ""categories""
                    WHERE
                        ""category_id"" = @Id;
                ";
        
            return await connection
                .QueryFirstOrDefaultAsync<Category>(
                    sql, new { Id = id })
                        ?? null;
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(Category), id);
    }
    
    public async Task<IEnumerable<Category?>> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "category_id");
        
            string sql = 
                $@"
                    SELECT
                        ""category_id""                 AS Id,
                    ""category_name""                   AS Name,
                        ""normalized_category_name""    AS NormalizedName,
                        ""category_name_ru""            AS NameRu,
                        ""normalized_category_name_ru"" AS NormalizedNameRu,
                        ""category_description""        AS Description,
                        ""is_active""                   AS IsActive,
                        ""created_date""                AS OccuredAt
                    FROM
                        ""categories""
                    WHERE
                        ""normalized_category_name"" 
                            LIKE @Name
                    OR 
                        ""normalized_category_name_ru"" 
                            LIKE @Name
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<Category>(
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

    public async Task<IEnumerable<Category?>> GetByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(Category), name);
        
        return result;
    }
}