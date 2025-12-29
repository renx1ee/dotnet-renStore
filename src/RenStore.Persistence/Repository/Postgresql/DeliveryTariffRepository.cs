using System.ComponentModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Shoes;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class DeliveryTariffRepository : IDeliveryTariffRepository
{
    private readonly ILogger<DeliveryTariffRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<DeliveryTariffSortBy, string> _sortColumnMapping = new()
    {
        { DeliveryTariffSortBy.Id, "delivery_tariff_id" }
    };
    
    public DeliveryTariffRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public DeliveryTariffRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(DeliveryTariffEntity tariff, CancellationToken cancellationToken)
    {
        var result = await _context.DeliveryTariffs.AddAsync(tariff, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(DeliveryTariffEntity tariff, CancellationToken cancellationToken)
    {
        var existingCategory = await this.GetByIdAsync(tariff.Id, cancellationToken);
        
        _context.DeliveryTariffs.Update(tariff);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var tariff = await this.GetByIdAsync(id, cancellationToken);
        this._context.DeliveryTariffs.Remove(tariff);
        await this._context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<DeliveryTariffEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTariffSortBy sortBy = DeliveryTariffSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "delivery_tariff_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""delivery_tariff_id"" AS Id,
                        ""price""              AS Price,
                        ""type""               AS Type,
                        ""description""        AS Description,
                        ""weight_limit_kg""    AS WeightLimitKg
                    FROM
                        ""delivery_tariffs""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<DeliveryTariffEntity>(sql, new
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
    
    public async Task<DeliveryTariffEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new InvalidEnumArgumentException();
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            string sql =
                @$"
                    SELECT
                        ""delivery_tariff_id"" AS Id,
                        ""price""              AS Price,
                        ""type""               AS Type,
                        ""description""        AS Description,
                        ""weight_limit_kg""    AS WeightLimitKg
                    FROM
                        ""delivery_tariffs""
                    WHERE
                        ""delivery_tariff_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryTariffEntity>(
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
    
    public async Task<DeliveryTariffEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryTariffEntity), id);
    }
}