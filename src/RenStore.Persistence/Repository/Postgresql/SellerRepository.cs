using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class SellerRepository : ISellerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<SellerSortBy, string> _sortColumnMapping = new()
    {
        { SellerSortBy.Id, "seller_id" },
        { SellerSortBy.Name, "seller_name" },
        { SellerSortBy.CreatedDate, "created_date" }
    };

    public SellerRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public SellerRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<long> CreateAsync(SellerEntity seller, CancellationToken cancellationToken)
    {
        await this._context.Sellers.AddAsync(seller, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return seller.Id;
    }
    
    public async Task UpdateAsync(SellerEntity seller, CancellationToken cancellationToken)
    {
        var existingSeller = await this.GetByIdAsync(seller.Id, cancellationToken);
        
        this._context.Update(seller);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var seller = await this.GetByIdAsync(id, cancellationToken);
        this._context.Sellers.Remove(seller);
        await this._context.SaveChangesAsync(cancellationToken);
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
                    ""seller_name""            AS Name,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS CreatedAt,
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
                    ""seller_name""            AS Name,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS CreatedAt,
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
                        ""seller_name""            AS Name,
                        ""normalized_seller_name"" AS NormalizedName,
                        ""seller_description""     AS Description,
                        ""created_date""           AS CreatedAt,
                        ""is_blocked""             AS IsBlocked,
                        ""user_id""                AS UserId
                    FROM
                        ""sellers""
                    WHERE
                        ""normalized_seller_name"" LIKE @Name
                ");
        
            var parameters = new DynamicParameters();
            parameters.Add("Name", $"%{name.ToUpper()}%");
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
                    ""seller_name""            AS Name,
                    ""normalized_seller_name"" AS NormalizedName,
                    ""seller_description""     AS Description,
                    ""created_date""           AS CreatedAt,
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
    */
}