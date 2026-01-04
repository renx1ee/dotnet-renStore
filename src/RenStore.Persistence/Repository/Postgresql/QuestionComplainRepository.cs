using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class QuestionComplainRepository : IQuestionComplainRepository
{
    private readonly ILogger<QuestionComplainRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<QuestionComplainSortBy, string> _sortColumnMapping = new()
    {
        { QuestionComplainSortBy.Id, "question_complain_id" }
    };

    public QuestionComplainRepository(
        ILogger<QuestionComplainRepository> logger,
        ApplicationDbContext context,
        string connectionString)
    {
        this._logger = logger;
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public QuestionComplainRepository(
        ILogger<QuestionComplainRepository> logger,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._logger = logger;
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(QuestionComplainEntity complain, CancellationToken cancellationToken)
    {
        var result = await this._context.QuestionComplains.AddAsync(complain, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task UpdateAsync(QuestionComplainEntity complain, CancellationToken cancellationToken)
    {
        var existingComplain = await this.GetByIdAsync(complain.Id, cancellationToken);
        
        _context.QuestionComplains.Update(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var complain = await this.GetByIdAsync(id, cancellationToken);
        this._context.QuestionComplains.Remove(complain);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<QuestionComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "question_complain_id");

            string sql =
                @$"
                    SELECT
                        ""question_complain_id"" AS Id,
                        ""reason""               AS Reason,
                        ""custom_reason""        AS CustomReason,
                        ""comment""              AS Comment,
                        ""created_date""         AS OccuredAt,
                        ""status""               AS Status,
                        ""resolved_date""        AS ResolvedAt,
                        ""moderator_comment""    AS ModeratorComment,
                        ""moderator_id""         AS ModeratorId,
                        ""product_question_id""  AS ProductQuestionId,
                        ""user_id""              AS UserId
                    FROM
                        ""question_complains""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<QuestionComplainEntity>(
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

    public async Task<QuestionComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT 
                        ""question_complain_id"" AS Id,
                        ""reason""               AS Reason,
                        ""custom_reason""        AS CustomReason,
                        ""comment""              AS Comment,
                        ""created_date""         AS OccuredAt,
                        ""status""               AS Status,
                        ""resolved_date""        AS ResolvedAt,
                        ""moderator_comment""    AS ModeratorComment,
                        ""moderator_id""         AS ModeratorId,
                        ""product_question_id""  AS ProductQuestionId,
                        ""user_id""              AS UserId
                    FROM
                        ""question_complains""
                    WHERE 
                        ""question_complain_id"" = @Id
                ";

            return await connection
                .QueryFirstOrDefaultAsync<QuestionComplainEntity>(
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

    public async Task<QuestionComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(cancellationToken: cancellationToken, id: id)
               ?? throw new NotFoundException(typeof(QuestionComplainEntity), id);
    }
    
    public async Task<IEnumerable<QuestionComplainEntity?>> FindByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "question_complain_id");
        
            string sql = 
                $@"
                    SELECT
                        ""question_complain_id"" AS Id,
                        ""reason""               AS Reason,
                        ""custom_reason""        AS CustomReason,
                        ""comment""              AS Comment,
                        ""created_date""         AS OccuredAt,
                        ""status""               AS Status,
                        ""resolved_date""        AS ResolvedAt,
                        ""moderator_comment""    AS ModeratorComment,
                        ""moderator_id""         AS ModeratorId,
                        ""product_question_id""  AS ProductQuestionId,
                        ""user_id""              AS UserId
                    FROM
                        ""question_complains""
                    WHERE
                        ""user_id"" = @UserId
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count
                    OFFSET @Offset;
                ";
        
            return await connection
                .QueryAsync<QuestionComplainEntity>(
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

    public async Task<IEnumerable<QuestionComplainEntity?>> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByUserIdAsync(userId, cancellationToken, sortBy, pageCount, page, descending);
        
        if (result is null || !result.Any())
            throw new NotFoundException(typeof(QuestionComplainEntity), userId);
        
        return result;
    }
}