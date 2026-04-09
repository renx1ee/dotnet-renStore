namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class CategoryQuery(CatalogDbContext context, ILogger logger) 
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase(context, logger),
      RenStore.Catalog.Application.Abstractions.Queries.ICategoryQuery
{
    private static readonly Dictionary<CategorySortBy, string> _categorySortColumnMapping = new()
    {
        { CategorySortBy.Name,      "normalized_name" },
        { CategorySortBy.NameRu,    "normalized_name_ru" },
        { CategorySortBy.CreatedAt, "created_date" },
        { CategorySortBy.Id,        "id" }
    };
    
    private static readonly Dictionary<SubCategorySortBy, string> _subCategorySortColumnMapping = new()
    {
        { SubCategorySortBy.Name,      "normalized_name" },
        { SubCategorySortBy.NameRu,    "normalized_name_ru" },
        { SubCategorySortBy.CreatedAt, "created_date" },
        { SubCategorySortBy.Id,        "id" }
    };
    
    // TODO: get all categories with sub categories
    
    #region Category
    
    public async Task<GetCategoryReadModel?> FindCategoryAsync(
        Guid categoryId,
        bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(categoryId));
        
        try
        {
            var sql = new StringBuilder(
                $"""
                 {GetCategorySql()}
                 WHERE "id" = @Id
                 """);

            sql.Append(includeDeleted 
                ? "LIMIT 1;" 
                : """AND "is_deleted" = false LIMIT 1;""");
            
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            return await connection
                .QueryFirstOrDefaultAsync<GetCategoryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new { Id = categoryId, IncludeDeleted = includeDeleted },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<PageResult<GetCategoryReadModel>> FindCategoriesAsync(
        CategorySortBy sortBy = CategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_categorySortColumnMapping.TryGetValue(sortBy, out var columnMapping))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
        
            var pageRequest = BuildPageRequest(
                page: page, 
                pageSize: pageSize, 
                descending: descending); 
            
            string whereClause = string.Empty;
            
            var parameters = new DynamicParameters();
            parameters.Add("Offset", pageRequest.Offset);
            parameters.Add("Sales", pageRequest.Limit);

            if (isDeleted.HasValue)
            {
                whereClause = """ AND "is_deleted" = @IsDeletedCategory """;
                parameters.Add("IsDeletedCategory", isDeleted);
            }
            
            var sql = 
                $"""
                 SELECT COUNT(*) FROM "categories" WHERE 1=1 {whereClause};

                 {GetCategorySql()}
                 WHERE 1=1 {whereClause}
                 ORDER BY "{columnMapping}" {pageRequest.Direction}
                 LIMIT @Sales
                 OFFSET @Offset; 
                 """;
            
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            var result =  await connection
                .QueryMultipleAsync(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: parameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

            var totalCount = await result.ReadFirstOrDefaultAsync<int>();
            var items = (await result.ReadAsync<GetCategoryReadModel>(buffered: false)).ToList();

            return new PageResult<GetCategoryReadModel>(
                Items: items,
                TotalCount: totalCount,
                Page: (int)page,
                PageSize: (int)pageSize);
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<PageResult<GetCategoryWithSubCategoryReadModel>> FindCategoriesWithSubCategoriesAsync(
        CategorySortBy sortBy = CategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeletedCategory = null,
        bool? isDeletedSubCategory = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_categorySortColumnMapping.TryGetValue(sortBy, out var columnMapping))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
        
            var pageRequest = BuildPageRequest( 
                page: page, 
                pageSize: pageSize, 
                descending: descending); 
        
            var whereClauseCategory = string.Empty; 
            var whereClauseSubCategory = string.Empty; 
            var parameters = new DynamicParameters(); 
            parameters.Add("Offset", pageRequest.Offset); 
            parameters.Add("Sales", pageRequest.Limit); 
            
            if (isDeletedCategory.HasValue)
            {
                whereClauseCategory = """ AND "is_deleted" = @IsDeletedCategory """;
                parameters.Add("IsDeletedCategory", isDeletedCategory);
            }
            
            if (isDeletedSubCategory.HasValue)
            {
                whereClauseSubCategory = """ AND "is_deleted" = @IsDeletedSubCategory """;
                parameters.Add("IsDeletedSubCategory", isDeletedSubCategory);
            }
            
            var sql = 
                $"""
                 SELECT COUNT(*) FROM "categories" WHERE 1=1 {whereClauseCategory};

                 {GetCategorySql()} 
                 WHERE 1=1 {whereClauseCategory} 
                 ORDER BY "{columnMapping}" {pageRequest.Direction} 
                 LIMIT @Sales 
                 OFFSET @Offset; 

                 {GetSubCategorySql()} 
                 WHERE "category_id" IN ( 
                     SELECT "id" 
                     FROM "categories" 
                     WHERE 1=1 {whereClauseCategory} 
                     ORDER BY "{columnMapping}" {pageRequest.Direction}
                     LIMIT @Sales 
                     OFFSET @Offset 
                 )
                 {whereClauseSubCategory};
                 """;
            
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            using var result =  await connection
                .QueryMultipleAsync(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: parameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
            
            var totalCount = await result.ReadFirstOrDefaultAsync<int>();
            
            var categories = (await result.ReadAsync<GetCategoryWithSubCategoryReadModel>(buffered: false))
                .ToDictionary(x => x.CategoryId);
            
            var subCategories = (await result.ReadAsync<GetSubCategoryReadModel>(buffered: false)).ToList();
            
            foreach (var subCategory in subCategories)
            {
                if (categories.TryGetValue(subCategory.CategoryId, out var category))
                    category.SubCategories.Add(subCategory);
            }
            
            return new PageResult<GetCategoryWithSubCategoryReadModel>(
                Items: categories.Values.ToList(),
                TotalCount: totalCount,
                Page: (int)page,
                PageSize: (int)pageSize);
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    #endregion
    
    #region SubCategory
    
    public async Task<GetSubCategoryReadModel?> FindSubCategoryAsync(
        Guid categoryId,
        Guid subCategoryId,
        bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(categoryId));
        
        if (subCategoryId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(subCategoryId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            var sql = new StringBuilder(
                $"""
                {GetSubCategorySql()}
                WHERE "id" = @Id
                AND "category_id" = @CategoryId
                """);
            
            sql.Append(includeDeleted 
                ? "LIMIT 1;" 
                : """AND "is_deleted" = false LIMIT 1;""");

            return await connection
                .QueryFirstOrDefaultAsync<GetSubCategoryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new { Id = subCategoryId, CategoryId = categoryId },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<PageResult<GetSubCategoryReadModel>> FindSubCategoriesAsync(
        Guid categoryId,
        SubCategorySortBy sortBy = SubCategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(categoryId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            
            if (!_subCategorySortColumnMapping.TryGetValue(sortBy, out var columnMapping))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
            var pageRequest = BuildPageRequest(
                page: page, 
                pageSize: pageSize, 
                descending: descending);
            
            string whereClause = string.Empty;
            
            var parameters = new DynamicParameters();
            parameters.Add("Id", categoryId);
            parameters.Add("Offset", pageRequest.Offset);
            parameters.Add("Sales", pageRequest.Limit);

            if (isDeleted.HasValue)
            {
                whereClause = """ AND "is_deleted" = @IsDeletedCategory """;
                parameters.Add("IsDeletedCategory", isDeleted);
            }
            
            var sql = 
                $"""
                 SELECT COUNT(*) FROM "sub_categories" WHERE 1=1 {whereClause};

                 {GetSubCategorySql()}
                 WHERE "category_id" = @Id {whereClause}
                 ORDER BY "{columnMapping}" {pageRequest.Direction}
                 LIMIT @Sales
                 OFFSET @Offset; 
                 """;
            
            var result =  await connection
                .QueryMultipleAsync(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: parameters,
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));

            var totalCount = await result.ReadFirstOrDefaultAsync<int>();
            var items = (await result.ReadAsync<GetSubCategoryReadModel>(buffered: false)).ToList();

            return new PageResult<GetSubCategoryReadModel>(
                Items: items,
                TotalCount: totalCount,
                Page: (int)page,
                PageSize: (int)pageSize);
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    #endregion

    private static string GetCategorySql()
    {
        return """
               SELECT
                   "id"          AS CategoryId,
                   "name"        AS Name,
                   "name_ru"     AS NameRu,
                   "description" AS Description,
                   "is_active"   AS IsActive
               FROM "categories"
               """;
    }
    
    private static string GetSubCategorySql()
    {
        return """
               SELECT
                   "id"          AS SubCategoryId,
                   "category_id" AS CategoryId,
                   "name"        AS Name,
                   "name_ru"     AS NameRu,
                   "description" AS Description,
                   "is_active"   AS IsActive
               FROM "sub_categories"
               """;
    }
}