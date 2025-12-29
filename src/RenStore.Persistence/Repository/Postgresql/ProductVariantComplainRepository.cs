using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductVariantComplainRepository : IProductVariantComplainRepository
{
    private readonly ILogger<ProductVariantComplainRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductVariantComplainSortBy, string> _sortColumnMapping = new()
    {
        { ProductVariantComplainSortBy.Id, "product_variant_complain_id" }
    };

    public ProductVariantComplainRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public ProductVariantComplainRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ProductVariantComplainEntity complain, CancellationToken cancellationToken)
    {
        var result = await this._context.ProductVariantComplains.AddAsync(complain, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(ProductVariantComplainEntity complain, CancellationToken cancellationToken)
    {
        var existingComplain = await this.GetByIdAsync(complain.Id, cancellationToken);
        
        _context.ProductVariantComplains.Update(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var complain = await this.GetByIdAsync(id, cancellationToken);
        this._context.ProductVariantComplains.Remove(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductVariantComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_variant_complain_id");

            string sql =
                @$"
                    SELECT
                        ""product_variant_complain_id"" AS Id,
                        ""reason""                      AS Reason,
                        ""custom_reason""               AS CustomReason,
                        ""comment""                     AS Comment,
                        ""created_date""                AS CreatedDate,
                        ""status""                      AS Status,
                        ""resolved_date""               AS ResolvedAt,
                        ""moderator_comment""           AS ModeratorComment,
                        ""moderator_id""                AS ModeratorId,
                        ""product_variant_id""          AS ProductVariantId,
                        ""user_id""                     AS UserId
                    FROM
                        ""product_variant_complains""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ProductVariantComplainEntity>(
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

    public async Task<ProductVariantComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT 
                        ""product_variant_complain_id"" AS Id,
                        ""reason""                      AS Reason,
                        ""custom_reason""               AS CustomReason,
                        ""comment""                     AS Comment,
                        ""created_date""                AS CreatedDate,
                        ""status""                      AS Status,
                        ""resolved_date""               AS ResolvedAt,
                        ""moderator_comment""           AS ModeratorComment,
                        ""moderator_id""                AS ModeratorId,
                        ""product_variant_id""          AS ProductVariantId,
                        ""user_id""                     AS UserId
                    FROM
                        ""product_variant_complains""
                    WHERE 
                        ""product_variant_complain_id"" = @Id
                ";

            return await connection
                .QueryFirstOrDefaultAsync<ProductVariantComplainEntity>(
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

    public async Task<ProductVariantComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(cancellationToken: cancellationToken, id: id)
               ?? throw new NotFoundException(typeof(ProductVariantComplainEntity), id);
    }
    
    public async Task<IEnumerable<ProductVariantComplainEntity?>> FindByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "product_variant_complain_id");
        
            string sql = 
                $@"
                    SELECT
                        ""product_variant_complain_id"" AS Id,
                        ""reason""                      AS Reason,
                        ""custom_reason""               AS CustomReason,
                        ""comment""                     AS Comment,
                        ""created_date""                AS CreatedDate,
                        ""status""                      AS Status,
                        ""resolved_date""               AS ResolvedAt,
                        ""moderator_comment""           AS ModeratorComment,
                        ""moderator_id""                AS ModeratorId,
                        ""product_variant_id""          AS ProductVariantId,
                        ""user_id""                     AS UserId
                    FROM
                        ""product_variant_complains""
                    WHERE
                        ""user_id"" = @UserId
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<ProductVariantComplainEntity>(
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

    public async Task<IEnumerable<ProductVariantComplainEntity?>> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByUserIdAsync(userId, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(ProductVariantComplainEntity), userId);
        
        return result;
    }
}