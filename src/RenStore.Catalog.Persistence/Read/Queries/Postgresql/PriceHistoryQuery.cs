namespace RenStore.Catalog.Persistence.Read.Queries.Postgresql;

internal sealed class PriceHistoryQuery
    : RenStore.Catalog.Persistence.Read.Base.DapperQueryBase,
      IPriceHistoryQuery
{
    private const string BaseSqlQuery =
        """
            SELECT 
                "id"               AS Id,
                "price"            AS Amount,
                "currency"         AS Currency,
                "valid_from"       AS ValidFrom,
                "is_active"        AS IsActive,
                "created_date"     AS CreatedAt,
                "deactivated_date" AS DeactivatedAt,
                "size_id"          AS SizeId
            FROM
                "price_history"
        """;

    private readonly Dictionary<PriceHistorySortBy, string> _sortColumnMapping = new()
    {
        { PriceHistorySortBy.Id,            "id" },
        { PriceHistorySortBy.Amount,        "price" },
        { PriceHistorySortBy.Currency,      "currency" },
        { PriceHistorySortBy.ValidFrom,     "valid_from" },
        { PriceHistorySortBy.CreatedAt,     "created_date" },
        { PriceHistorySortBy.DeactivatedAt, "deactivated_date" },
    };
    
    public PriceHistoryQuery(
        ILogger<PriceHistoryQuery> logger,
        CatalogDbContext context)
        : base(context, logger)
    {
    }
    
    public async Task<IReadOnlyList<PriceHistoryReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isActive = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageCount, descending);

            var sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                ");
            
            if(isActive.HasValue)
                sql.Append(@$" WHERE ""is_active"" = @IsActive");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset");

            var result = await connection
                .QueryAsync<PriceHistoryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
                            IsActive = isActive
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

    public async Task<PriceHistoryReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql =
                $@"
                    {BaseSqlQuery}
                    WHERE ""id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<PriceHistoryReadModel>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        commandTimeout: CommandTimeoutSeconds,
                        transaction: CurrentDbTransaction,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<PriceHistoryReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(
            id: id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(typeof(PriceHistoryReadModel), id);
    }
    
    public async Task<IReadOnlyList<PriceHistoryReadModel>> FindBySizeIdAsync(
        Guid sizeId,
        CancellationToken cancellationToken,
        PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isActive = null)
    {
        if (sizeId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sizeId));
        
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);

            var sql = new StringBuilder(
                $@"
                    {BaseSqlQuery}
                    WHERE ""size_id"" = @SizeId
                ");
            
            if(isActive.HasValue)
                sql.Append(@$" AND ""is_active"" = @IsActive");

            sql.Append($@" ORDER BY ""{columnName}"" {pageRequest.Direction}
                           LIMIT @Count
                           OFFSET @Offset");

            var result = await connection
                .QueryAsync<PriceHistoryReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Offset = pageRequest.Offset,
                            Count = pageRequest.Limit,
                            IsActive = isActive,
                            SizeId = sizeId
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
}