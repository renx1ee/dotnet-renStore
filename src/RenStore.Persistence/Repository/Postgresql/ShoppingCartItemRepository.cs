using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class ShoppingCartItemRepository : IShoppingCartItemRepository
{
    private readonly string _connectionString;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ShoppingCartItemRepository> _logger;
    
    private readonly Dictionary<ShoppingCartItemSortBy, string> _sorcColumnMapping =
        new()
        {
            { ShoppingCartItemSortBy.Id, "cart_item_id" }
        };

    public ShoppingCartItemRepository(
        ApplicationDbContext context,
        string connectionString,
        ILogger<ShoppingCartItemRepository> logger)
    {
        this._context = context;
        this._logger = logger;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));;
    }
    
    public ShoppingCartItemRepository(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<ShoppingCartItemRepository> logger)
    {
        this._context = context;
        this._logger = logger;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(ShoppingCartItemEntity item, CancellationToken cancellationToken)
    {
        await this._context.ShoppingCartItems.AddAsync(item, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
    
    public async Task UpdateAsync(ShoppingCartItemEntity item, CancellationToken cancellationToken)
    {
        var existingItem = await this.GetByIdAsync(item.Id, cancellationToken);
        
        this._context.Update(item);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var cart = await this.GetByIdAsync(id, cancellationToken);
        this._context.ShoppingCartItems.Remove(cart);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ShoppingCartItemEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
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
            var columnName = _sorcColumnMapping.GetValueOrDefault(sortBy, "cart_item_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                        ""cart_item_id"" AS Id,
                        ""quantity"" AS Quantity,
                        ""price"" AS Price,
                        ""cart_id"" AS CartId,
                        ""product_id"" AS ProductId
                    FROM
                        ""cart_items"" 
                ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);
        
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryAsync<ShoppingCartItemEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with cart {e.Message}");
        }
    }
    
    public async Task<ShoppingCartItemEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT
                        ""cart_item_id"" AS Id,
                        ""quantity"" AS Quantity,
                        ""price"" AS Price,
                        ""cart_id"" AS CartId,
                        ""product_id"" AS ProductId
                    FROM
                        ""cart_items"" 
                    WHERE
                        ""cart_id"" = @Id;
            ";
        
            return await connection
                .QueryFirstOrDefaultAsync<ShoppingCartItemEntity>(
                    sql, new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<ShoppingCartItemEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(ShoppingCartItemEntity), id);
    }
    
    public async Task<IEnumerable<ShoppingCartItemEntity>> FindByCartIdAsync(
        Guid cartId,
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false)
    {
        if (cartId == Guid.Empty)
            return new List<ShoppingCartItemEntity>(); // TODO: 
        
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            var columnName = _sorcColumnMapping.GetValueOrDefault(sortBy, "cart_item_id");
            string direction = descending ? "DESC" : "ASC";
        
            var sql = new StringBuilder(
                @"
                    SELECT
                        ""cart_item_id"" AS Id,
                        ""quantity"" AS Quantity,
                        ""price"" AS Price,
                        ""cart_id"" AS CartId,
                        ""product_id"" AS ProductId
                    FROM
                        ""cart_items"" 
                    WHERE ""cart_id"" = @CartId
                ");
            
            var parameters = new DynamicParameters();
            parameters.Add("Count", (int)pageCount);
            parameters.Add("Offset", (int)offset);
            parameters.Add("CartId", cartId);
        
            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");
        
            return await connection
                .QueryAsync<ShoppingCartItemEntity>(
                    sql.ToString(), 
                    parameters);
        }
        catch (NpgsqlException e)
        {
            throw new Exception($"Database error with cart {e.Message}");
        }
    }
    
    public async Task<IEnumerable<ShoppingCartItemEntity>> GetByCartIdAsync(
        Guid cartId,
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false)
    {
        return await this.FindByCartIdAsync(
                   cartId: cartId,
                   cancellationToken: cancellationToken,
                   sortBy: sortBy,
                   pageCount: pageCount,
                   page: page,
                   descending: descending) 
               ?? throw new NotFoundException(typeof(ShoppingCartItemEntity), cartId);
    }
}