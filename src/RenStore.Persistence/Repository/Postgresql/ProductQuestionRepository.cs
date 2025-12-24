/*using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductQuestionRepository

{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductQuestionSortBy, string> _sortColumnMapping = 
        new()
        {
            { ProductQuestionSortBy.Id, "question_id" },
            { ProductQuestionSortBy.CreatedDate, "created_date" }
        };

    public ProductQuestionRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductQuestionRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ProductQuestionEntity question, CancellationToken cancellationToken)
    {
        await this._context.ProductQuestions.AddAsync(question, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return question.Id;
    }
    
    public async Task UpdateAsync(ProductQuestionEntity question, CancellationToken cancellationToken)
    {
        var existingQuestion = await this.GetByIdAsync(question.Id, cancellationToken);
        
        this._context.Update(question);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var question = await this.GetByIdAsync(id, cancellationToken);
        this._context.ProductQuestions.Remove(question);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductQuestionEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductQuestionSortBy sortBy = ProductQuestionSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false,
        bool? isApproved = null)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "question_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                       ""question_id""        AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""product_variant_id"" AS ProductVariantId,
                       ""user_id""            AS UserId
                    FROM
                       ""product_questions""
            ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);

            if (isApproved.HasValue)
            {
                sql.Append(@" WHERE ""is_approved"" = @IsApproved");
                parameters.Add("IsApproved", isApproved.Value);
            }
        
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryAsync<ProductQuestionEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with question {e.Message}");
        }
    }
    
    public async Task<ProductQuestionEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                       ""question_id""        AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""product_variant_id"" AS ProductVariantId,
                       ""user_id""            AS UserId
                    FROM
                       ""product_questions""
                    WHERE
                        ""question_id"" = @Id;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ProductQuestionEntity>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<ProductQuestionEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductQuestionEntity), id);
    }
    
    public async Task<ProductQuestionEntity?> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                       ""question_id""        AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""product_variant_id"" AS ProductVariantId,
                       ""user_id""            AS UserId
                    FROM
                       ""product_questions""
                    WHERE
                        ""user_id"" = @UserId;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ProductQuestionEntity>(
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
    
    public async Task<ProductQuestionEntity> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await this.FindByUserIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductQuestionEntity), userId);
    }   
}*/