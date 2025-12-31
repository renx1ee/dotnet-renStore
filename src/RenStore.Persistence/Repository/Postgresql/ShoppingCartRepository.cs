using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly string _connectionString;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ShoppingCartRepository> _logger;

    private readonly Dictionary<ShoppingCartSortBy, string> _sorcColumnMapping = new()
    {
        { ShoppingCartSortBy.Id, "cart_id" }
    };
    
    public ShoppingCartRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));;
    }
    
    public ShoppingCartRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ShoppingCartEntity cart, CancellationToken cancellationToken)
    {
        await this._context.ShoppingCarts.AddAsync(cart, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return cart.Id;
    }
    
    public async Task UpdateAsync(ShoppingCartEntity cart, CancellationToken cancellationToken)
    {
        var existingCart = await this.GetByIdAsync(cart.Id, cancellationToken);
        
        this._context.ShoppingCarts.Update(cart);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var cart = await this.GetByIdAsync(id, cancellationToken);
        this._context.ShoppingCarts.Remove(cart);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ShoppingCartEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
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
            var columnName = _sorcColumnMapping.GetValueOrDefault(sortBy, "cart_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                        ""cart_id""      AS Id,
                        ""total_price""  AS TotalPrice,
                        ""created_date"" AS CreatedAt,
                        ""updated_date"" AS UpdatedAt,
                        ""user_id""      AS UserId
                    FROM
                        ""shopping_carts"" 
                ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);
        
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryAsync<ShoppingCartEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with cart {e.Message}");
        }
    }
    
    public async Task<ShoppingCartEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                        ""cart_id""      AS Id,
                        ""total_price""  AS TotalPrice,
                        ""created_date"" AS CreatedAt,
                        ""updated_date"" AS UpdatedAt,
                        ""user_id""      AS UserId
                    FROM
                        ""shopping_carts"" 
                    WHERE
                        ""cart_id"" = @Id;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ShoppingCartEntity>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<ShoppingCartEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ShoppingCartEntity), id);
    }
    
    public async Task<ShoppingCartEntity?> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false)
    {
        if (string.IsNullOrEmpty(userId)) 
            return new ShoppingCartEntity(); // TODO: 
        
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var columnName = _sorcColumnMapping.GetValueOrDefault(sortBy, "cart_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                        ""cart_id""      AS Id,
                        ""total_price""  AS TotalPrice,
                        ""created_date"" AS CreatedAt,
                        ""updated_date"" AS UpdatedAt,
                        ""user_id""      AS UserId
                    FROM
                        ""shopping_carts"" 
                    WHERE ""user_id"" = @UserId
                ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);
            parameters.Add("UserId", userId);
        
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryFirstOrDefaultAsync<ShoppingCartEntity>(
                    sql.ToString(),
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with cart {e.Message}");
        }
    }
    
    public async Task<ShoppingCartEntity?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
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
               ?? throw new NotFoundException(typeof(ShoppingCartEntity), userId);
    }
}