/*using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductAnswerRepository

{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductAnswerSortBy, string> _sortColumnMapping = 
        new()
        {
            { ProductAnswerSortBy.Id, "answer_id" },
            { ProductAnswerSortBy.CreatedDate, "created_date" }
        };

    public ProductAnswerRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ProductAnswerRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ProductAnswerEntity productAnswerEntity, CancellationToken cancellationToken)
    {
        await this._context.ProductAnswers.AddAsync(productAnswerEntity, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return productAnswerEntity.Id;
    }
    
    public async Task UpdateAsync(ProductAnswerEntity answer, CancellationToken cancellationToken)
    {
        var existingAnswer = await this.GetByIdAsync(answer.Id, cancellationToken);
        
        this._context.Update(answer);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var answer = await this.GetByIdAsync(id, cancellationToken);
        this._context.ProductAnswers.Remove(answer);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductAnswerEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductAnswerSortBy sortBy = ProductAnswerSortBy.Id,
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
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "seller_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                       ""answer_id""          AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""seller_id""          AS SellerId,
                       ""question_id""        AS QuestionId
                    FROM
                       ""product_answers""
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
                .QueryAsync<ProductAnswerEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with seller {e.Message}");
        }
    }
    
    public async Task<ProductAnswerEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                       ""answer_id""          AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""seller_id""          AS SellerId,
                       ""question_id""        AS QuestionId
                    FROM
                       ""product_answers""
                    WHERE
                        ""answer_id"" = @Id;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ProductAnswerEntity>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<ProductAnswerEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductAnswerEntity), id);
    }
    
    public async Task<ProductAnswerEntity?> FindBySellerIdAsync(string sellerId, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                       ""answer_id""          AS Id,
                       ""message""            AS Message,
                       ""created_date""       AS CreatedDate,
                       ""moderated_date""     AS ModeratedDate,
                       ""is_approved""        AS IsApproved,
                       ""seller_id""          AS SellerId,
                       ""question_id""        AS QuestionId
                    FROM
                       ""product_answers""
                    WHERE
                        ""seller_id"" = @SellerId;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ProductAnswerEntity>(
                    sql, new
                    {
                        SellerId = sellerId
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<ProductAnswerEntity> GetBySellerIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await this.FindBySellerIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(typeof(ProductAnswerEntity), userId);
    }   
}*/