using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ReviewSortBy, string> _sortColumnMapping = new()
    {
        { ReviewSortBy.Id, "review_id" },
        { ReviewSortBy.CreatedDate, "created_date" }
    };
    
    public ReviewRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ReviewRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ReviewEntity review, CancellationToken cancellationToken)
    {
        var result = await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(ReviewEntity review, CancellationToken cancellationToken)
    {
        var reviewExists = await this.GetByIdAsync(review.Id, cancellationToken);
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var review = await this.GetByIdAsync(id, cancellationToken);
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ReviewEntity?>> FindAllAsync(
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "country_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                $@"
                    SELECT
                        ""review_id"" AS Id,
                        ""message"" AS Message,
                        ""rating"" AS ReviewRating,
                        ""created_date"" AS CreatedAt,
                        ""last_updated_date"" AS LastUpdatedDate,
                        ""is_updated"" AS IsUpdated,
                        ""moderated_date"" AS ModeratedDate,
                        ""status"" AS Status,
                        ""is_approved"" AS IsApproved,
                        ""user_id"" AS UserId,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""reviews""
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ReviewEntity?>(
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

    public async Task<ReviewEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT
                        ""review_id"" AS Id,
                        ""message"" AS Message,
                        ""rating"" AS ReviewRating,
                        ""created_date"" AS CreatedAt,
                        ""last_updated_date"" AS LastUpdatedDate,
                        ""is_updated"" AS IsUpdated,
                        ""moderated_date"" AS ModeratedDate,
                        ""status"" AS Status,
                        ""is_approved"" AS IsApproved,
                        ""user_id"" AS UserId,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""reviews""
                    WHERE
                        ""review_id"" = @Id;
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<ReviewEntity>(
                    sql, new { Id = id });   
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<ReviewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ReviewEntity), id);
    }
    
    public async Task<IEnumerable<ReviewEntity?>> FindByProductVariantIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "country_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                $@"
                    SELECT
                        ""review_id"" AS Id,
                        ""message"" AS Message,
                        ""rating"" AS ReviewRating,
                        ""created_date"" AS CreatedAt,
                        ""last_updated_date"" AS LastUpdatedDate,
                        ""is_updated"" AS IsUpdated,
                        ""moderated_date"" AS ModeratedDate,
                        ""status"" AS Status,
                        ""is_approved"" AS IsApproved,
                        ""user_id"" AS UserId,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""reviews""
                    WHERE 
                        ""product_variant_id"" = @ProductId
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ReviewEntity?>(
                    sql, new
                    {
                        Count = (int)pageCount,
                        Offset = (int)offset,
                        ProductId = productId
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }


    public async Task<IEnumerable<ReviewEntity?>> GetByProductVariantIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        return await this.FindByProductVariantIdAsync(
                   productId: productId,
                   cancellationToken: cancellationToken,
                   sortBy: sortBy,
                   pageCount: pageCount,
                   page: page,
                   descending: descending)
               ?? throw new NotFoundException(typeof(ReviewEntity), productId);
    }
    
    public async Task<IEnumerable<ReviewEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "country_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                $@"
                    SELECT
                        ""review_id"" AS Id,
                        ""message"" AS Message,
                        ""rating"" AS ReviewRating,
                        ""created_date"" AS CreatedAt,
                        ""last_updated_date"" AS LastUpdatedDate,
                        ""is_updated"" AS IsUpdated,
                        ""moderated_date"" AS ModeratedDate,
                        ""status"" AS Status,
                        ""is_approved"" AS IsApproved,
                        ""user_id"" AS UserId,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""reviews""
                    WHERE 
                        ""product_variant_id"" = @UserId
                    ORDER BY 
                        {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<ReviewEntity?>(
                    sql, new
                    {
                        Count = (int)pageCount,
                        Offset = (int)offset,
                        UserId = userId
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}"); 
        }
    }


    public async Task<IEnumerable<ReviewEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        return await this.FindByUserIdAsync(
                   userId: userId,
                   cancellationToken: cancellationToken,
                   sortBy: sortBy,
                   pageCount: pageCount,
                   page: page,
                   descending: descending)
               ?? throw new NotFoundException(typeof(ReviewEntity), userId);
    }
}