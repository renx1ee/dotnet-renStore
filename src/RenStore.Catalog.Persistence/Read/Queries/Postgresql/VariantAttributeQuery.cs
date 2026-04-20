namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class VariantAttributeQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IVariantAttributeQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                "id"           AS Id,
                "key"          AS Key,
                "value"        AS Value,
                "is_deleted"   AS IsDeletedCategory,
                "created_date" AS CreatedAt,
                "updated_date" AS UpdatedAt,
                "deleted_date" AS DeletedAt,
                "variant_id"   AS VariantId,
                "version"      AS Version
            FROM
                "variant_attributes"
        """;

    private static readonly Dictionary<VariantAttributeSortBy, string> _sortColumnMapping = new()
    {
        { VariantAttributeSortBy.Id,        "id" },
        { VariantAttributeSortBy.Key,       "key" },
        { VariantAttributeSortBy.Value,     "value" },
        { VariantAttributeSortBy.CreatedAt, "created_date" },
        { VariantAttributeSortBy.UpdatedAt, "updated_date" },
        { VariantAttributeSortBy.DeletedAt, "deleted_date" },
        { VariantAttributeSortBy.Version,   "version" }
    };

    public VariantAttributeQuery(
        ILogger<VariantAttributeQuery> logger,
        CatalogDbContext context) 
        : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<VariantAttributeReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);

            StringBuilder sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append(" WHERE \"is_deleted\" = @IsDeletedCategory");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset");

            var result = await connection
                .QueryAsync<VariantAttributeReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
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

    public async Task<VariantAttributeReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""id"" = @Id;
                ";

            return await connection.QueryFirstOrDefaultAsync(
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

    public async Task<VariantAttributeReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id, 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(VariantAttributeReadModel), id);
    }
    
    public async Task<IReadOnlyList<VariantAttributeReadModel>> FindByVariantIdAsync(
        CancellationToken cancellationToken,
        Guid variantId,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);

            StringBuilder sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                    WHERE ""variant_id"" = @VariantId
                ");
            
            if (isDeleted.HasValue)
                sql.Append(" AND \"is_deleted\" = @IsDeletedCategory");
            
            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset");

            var result = await connection
                .QueryAsync<VariantAttributeReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            VariantId = variantId
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
    
    public async Task<IReadOnlyList<VariantAttributeReadModel>> FindByKeyAsync(
        CancellationToken cancellationToken,
        string key,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentOutOfRangeException(nameof(key));
        
        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            if (_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);

            StringBuilder sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                    WHERE ""key"" = @Key
                ");
            
            if (isDeleted.HasValue)
                sql.Append(" AND \"is_deleted\" = @IsDeletedCategory");
            
            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset");

            var result = await connection
                .QueryAsync<VariantAttributeReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted,
                            Key = key
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
    
    // TODO: search by value or key
}