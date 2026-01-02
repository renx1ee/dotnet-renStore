using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class SellerComplainRepository : ISellerComplainRepository
{
    private readonly ILogger<SellerComplainRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    private readonly Dictionary<SellerComplainSortBy, string> _sortColumnMapping = new()
    {
        { SellerComplainSortBy.Id, "seller_complain_id" }
    };

    public SellerComplainRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public SellerComplainRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(SellerComplainEntity complain, CancellationToken cancellationToken)
    {
        var result = await this._context.SellerComplains.AddAsync(complain, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(SellerComplainEntity complain, CancellationToken cancellationToken)
    {
        var existingComplain = await this.GetByIdAsync(complain.Id, cancellationToken);
        
        _context.SellerComplains.Update(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var complain = await this.GetByIdAsync(id, cancellationToken);
        this._context.SellerComplains.Remove(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<SellerComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "seller_complain_id");

            string sql =
                @$"
                    SELECT
                        ""seller_complain_id"" AS Id,
                        ""reason""             AS Reason,
                        ""custom_reason""      AS CustomReason,
                        ""comment""            AS Comment,
                        ""created_date""       AS CreatedAt,
                        ""status""             AS Status,
                        ""resolved_date""      AS ResolvedAt,
                        ""moderator_comment""  AS ModeratorComment,
                        ""moderator_id""       AS ModeratorId,
                        ""seller_id""          AS SellerId,
                        ""user_id""            AS UserId
                    FROM
                        ""seller_complains""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<SellerComplainEntity>(
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

    public async Task<SellerComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT
                        ""seller_complain_id"" AS Id,
                        ""reason""             AS Reason,
                        ""custom_reason""      AS CustomReason,
                        ""comment""            AS Comment,
                        ""created_date""       AS CreatedAt,
                        ""status""             AS Status,
                        ""resolved_date""      AS ResolvedAt,
                        ""moderator_comment""  AS ModeratorComment,
                        ""moderator_id""       AS ModeratorId,
                        ""seller_id""          AS SellerId,
                        ""user_id""            AS UserId
                    FROM
                        ""seller_complains""
                    WHERE 
                        ""seller_complain_id"" = @Id
                ";

            return await connection
                .QueryFirstOrDefaultAsync<SellerComplainEntity>(
                    sql, new
                    {
                        Id = id
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<SellerComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(cancellationToken: cancellationToken, id: id)
               ?? throw new NotFoundException(typeof(SellerComplainEntity), id);
    }
    
    public async Task<IEnumerable<SellerComplainEntity?>> FindByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        if(string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(nameof(userId));

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "seller_complain_id");
        
            string sql = 
                $@"
                    SELECT
                        ""seller_complain_id"" AS Id,
                        ""reason""             AS Reason,
                        ""custom_reason""      AS CustomReason,
                        ""comment""            AS Comment,
                        ""created_date""       AS CreatedAt,
                        ""status""             AS Status,
                        ""resolved_date""      AS ResolvedAt,
                        ""moderator_comment""  AS ModeratorComment,
                        ""moderator_id""       AS ModeratorId,
                        ""seller_id""          AS SellerId,
                        ""user_id""            AS UserId
                    FROM
                        ""seller_complains""
                    WHERE
                        ""user_id"" = @UserId
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<SellerComplainEntity>(
                    sql, new
                    {
                        UserId = userId,
                        Count = (int)pageCount,
                        Offset = (int)offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<SellerComplainEntity?>> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByUserIdAsync(userId, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(SellerComplainEntity), userId);
        
        return result;
    }
}