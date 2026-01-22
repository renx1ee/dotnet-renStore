/*using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class SellerRepository(
    ILogger<SellerRepository> logger,
    ApplicationDbContext context)
{
    private const int MaxPageSize = 1000;
    private const uint CommandTimeoutSeconds = 30;
    
    private const string BaseSqlQuery = 
        """
        
        """;
    
    private readonly Dictionary<SellerSortBy, string> _sortColumnMapping = new()
    {
        { SellerSortBy.Id, "seller_id" },
        { SellerSortBy.Key, "seller_name" },
        { SellerSortBy.CreatedAt, "created_date" }
    };
        
    private readonly ApplicationDbContext _context     = context 
                                                         ?? throw new ArgumentNullException(nameof(context));
    
    private readonly ILogger<SellerRepository> _logger = logger 
                                                         ?? throw new ArgumentNullException(nameof(logger));
    
    private DbTransaction? CurrentDbTransaction =>
        this._context.Database.CurrentTransaction?.GetDbTransaction();

    public async Task<long> CreateAsync(
        SellerEntity seller,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(seller);

        seller.OccuredAt = DateTimeOffset.UtcNow;

        var result = await this._context.Sellers.AddAsync(seller, cancellationToken);
        
        return result.Entity.Id;
    }

    public async Task CreateRangeAsync(
        IReadOnlyCollection<SellerEntity> sellers,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sellers);

        var sellersList = sellers as IList<SellerEntity> ?? sellers.ToList();

        if (!sellers.Any()) return;

        foreach (var seller in sellers)
        {
            seller.OccuredAt = DateTimeOffset.UtcNow;
        }
    }
    
    
    public async Task<IEnumerable<SellerEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "seller_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                SELECT
                    ""seller_id""              AS Id,
                    ""seller_name""            AS Key,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS OccuredAt,
                    ""is_blocked""             AS IsBlocked,
                    ""user_id""                AS UserId
                FROM
                    ""sellers""
            ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);

            if (isBlocked.HasValue)
            {
                sql.Append(@" WHERE ""is_blocked"" = @IsBlocked");
                parameters.Add("IsBlocked", isBlocked.Value);
            }
            
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryAsync<SellerEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with seller {e.Message}");
        }
    }
    
    public async Task<SellerEntity?> FindByIdAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                SELECT
                    ""seller_id""              AS Id,
                    ""seller_name""            AS Key,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS OccuredAt,
                    ""is_blocked""             AS IsBlocked,
                    ""user_id""                AS UserId
                FROM
                    ""sellers""
                WHERE
                    ""seller_id"" = @Id;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<SellerEntity>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<SellerEntity> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(SellerEntity), id);
    }
    
    public async Task<IEnumerable<SellerEntity>> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));
        
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "seller_id");
            var direction = descending ? "DESC" : "ASC";

            var sql = new StringBuilder(
                @"
                    SELECT
                        ""seller_id""              AS Id,
                        ""seller_name""            AS Key,
                        ""normalized_seller_name"" AS NormalizedName,
                        ""seller_description""     AS Description,
                        ""created_date""           AS OccuredAt,
                        ""is_blocked""             AS IsBlocked,
                        ""user_id""                AS UserId
                    FROM
                        ""sellers""
                    WHERE
                        ""normalized_seller_name"" LIKE @Key
                ");
        
            var parameters = new DynamicParameters();
            parameters.Add("Key", $"%{name.ToUpper()}%");
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);

            if (isBlocked.HasValue)
            {
                parameters.Add("IsBlocked", isBlocked);
                sql.Append(@" AND ""is_blocked"" = @IsBlocked");
            }
            
            sql.Append($" ORDER BY {columnName} {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
               .QueryAsync<SellerEntity>(
                   sql.ToString(), 
                   parameters)
                       ?? [];
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<IEnumerable<SellerEntity>> GetByNameAsync(string name, 
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null)
    {
        return await this.FindByNameAsync(name, cancellationToken, sortBy, pageCount, page, descending, isBlocked)
            ?? throw new NotFoundException(typeof(SellerEntity), name);
    }
    
    public async Task<SellerEntity?> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                SELECT
                    ""seller_id""              AS Id,
                    ""seller_name""            AS Key,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS OccuredAt,
                    ""is_blocked""             AS IsBlocked,
                    ""user_id""                AS UserId
                FROM
                    ""sellers""
                WHERE
                    ""user_id"" = @UserId;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<SellerEntity>(
                    sql, new
                    {
                        UserId = userId
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<SellerEntity> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await this.FindByUserIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(typeof(SellerEntity), userId);
    }   
    /*
    // TODO:
    public async Task<SellerEntity?> FindByCreatedDateRangeAsync(CancellationToken cancellationToken)
    {
        return null;
    }
    // TODO:
    public async Task<SellerEntity> GetByDateCreatedRangeAsync(CancellationToken cancellationToken)
    {
        return null;
    }
    #1#
}*/