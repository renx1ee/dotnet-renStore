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

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<SubCategorySortBy, string> _sortColumnMapping = new()
    {
        { SubCategorySortBy.Id, "sub_category_id" },
        { SubCategorySortBy.Name, "normalized_sub_category_name" },
        { SubCategorySortBy.NameRu, "normalized_sub_category_name_ru" },
    };
    
    public SubCategoryRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public SubCategoryRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<int> CreateAsync(SubCategory subCategory, CancellationToken cancellationToken)
    {
        var result = await this._context.SubCategories.AddAsync(subCategory, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(SubCategory subCategory, CancellationToken cancellationToken)
    {
        var existingSubCategory = await this.GetByIdAsync(subCategory.Id, cancellationToken);
        
        _context.SubCategories.Update(subCategory);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var subCategory = await this.GetByIdAsync(id, cancellationToken);
        this._context.SubCategories.Remove(subCategory);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<SubCategory>> FindAllAsync(
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
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
                        ""sub_category_id""                 AS Id,
                        ""sub_category_name""               AS Key,
                        ""normalized_sub_category_name""    AS NormalizedName,
                        ""sub_category_name_ru""            AS NameRu,
                        ""normalized_sub_category_name_ru"" AS NormalizedNameRu,
                        ""sub_category_description""        AS Description,
                        ""is_active""                       AS IsActive,
                        ""created_date""                    AS OccuredAt,
                        ""category_id""                     As CategoryId
                    FROM
                        ""sub_categories"" 
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<SubCategory>(
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
    
    public async Task<SubCategory?> FindByIdAsync(
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
                        ""sub_category_id""                 AS Id,
                        ""sub_category_name""               AS Key,
                        ""normalized_sub_category_name""    AS NormalizedName,
                        ""sub_category_name_ru""            AS NameRu,
                        ""normalized_sub_category_name_ru"" AS NormalizedNameRu,
                        ""sub_category_description""        AS Description,
                        ""is_active""                       AS IsActive,
                        ""created_date""                    AS OccuredAt,
                        ""category_id""                     As CategoryId
                    FROM
                        ""sub_categories""
                    WHERE
                        ""sub_category_id"" = @Id;
                ";
        
            return await connection
                .QueryFirstOrDefaultAsync<SubCategory>(
                    sql, new { Id = id })
                        ?? null;
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<SubCategory> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(Category), id);
    }
    
    public async Task<IEnumerable<SubCategory?>> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "sub_category_id");
        
            string sql = 
                $@"
                    SELECT
                        ""sub_category_id""                 AS Id,
                        ""sub_category_name""               AS Key,
                        ""normalized_sub_category_name""    AS NormalizedName,
                        ""sub_category_name_ru""            AS NameRu,
                        ""normalized_sub_category_name_ru"" AS NormalizedNameRu,
                        ""sub_category_description""        AS Description,
                        ""is_active""                       AS IsActive,
                        ""created_date""                    AS OccuredAt,
                        ""category_id""                     As CategoryId
                    FROM
                        ""sub_categories""
                    WHERE
                        ""normalized_sub_category_name"" 
                            LIKE @Key
                    OR 
                        ""normalized_sub_category_name_ru"" 
                            LIKE @Key
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<SubCategory>(
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

    public async Task<IEnumerable<SubCategory?>> GetByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(SubCategory), name);
        
        return result;
    }
    
    public async Task<IEnumerable<SubCategory?>> FindByCategoryIdAsync(
        int categoryId, 
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        if(categoryId <= 0)
            throw new ArgumentException(nameof(categoryId));

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "sub_category_id");
        
            string sql = 
                $@"
                    SELECT
                        ""sub_category_id""                 AS Id,
                        ""sub_category_name""               AS Key,
                        ""normalized_sub_category_name""    AS NormalizedName,
                        ""sub_category_name_ru""            AS NameRu,
                        ""normalized_sub_category_name_ru"" AS NormalizedNameRu,
                        ""sub_category_description""        AS Description,
                        ""is_active""                       AS IsActive,
                        ""created_date""                    AS OccuredAt,
                        ""category_id""                     As CategoryId
                    FROM
                        ""sub_categories""
                    WHERE
                        ""category_id"" 
                            LIKE @CategoryId
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<SubCategory>(
                    sql, new
                    {
                        CategoryId = categoryId,
                        Count = (int)pageCount,
                        Offset = (int)offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<SubCategory?>> GetByCategoryIdAsync(
        int categoryId, 
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByCategoryIdAsync(categoryId, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(SubCategory), categoryId);
        
        return result;
    }
}