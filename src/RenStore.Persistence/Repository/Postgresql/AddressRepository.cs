using System.ComponentModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class AddressRepository : IAddressRepository
{
    private readonly ILogger<AddressRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<AddressSortBy, string> _sortColumnMapping = new()
    {
        { AddressSortBy.Id, "address_id" },
        { AddressSortBy.HouseCode, "house_code" },
        { AddressSortBy.FlatNumber, "flat_number" },
        { AddressSortBy.Street, "street" }
    };
    
    public AddressRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public AddressRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<Guid> CreateAsync(AddressEntity address, CancellationToken cancellationToken)
    {
        var result = await _context.Addresses.AddAsync(address, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(AddressEntity address, CancellationToken cancellationToken)
    {
        var existingAddress = await this.GetByIdAsync(address.Id, cancellationToken);
        
        _context.Addresses.Update(address);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var address = await this.GetByIdAsync(id, cancellationToken);
        this._context.Addresses.Remove(address);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AddressEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
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
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "address_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""address_id""       AS Id,
                        ""house_code""       AS HouseCode,
                        ""street""           AS Street,
                        ""building_number""  AS BuildingNumber,
                        ""apartment_number"" AS ApartmentNumber,
                        ""entrance""         AS Entrance,
                        ""floor""            AS Floor,
                        ""flat_number""      AS FlatNumber,
                        ""full_address""     AS FullAddress,
                        ""created_date""     AS CreatedDate,
                        ""user_id""          AS ApplicationUserId,
                        ""country_id""       AS CountryId,
                        ""city_id""          AS CityId
                    FROM
                        ""addresses""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<AddressEntity>(sql, new
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

    public async Task<AddressEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
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
                        ""address_id""       AS Id,
                        ""house_code""       AS HouseCode,
                        ""street""           AS Street,
                        ""building_number""  AS BuildingNumber,
                        ""apartment_number"" AS ApartmentNumber,
                        ""entrance""         AS Entrance,
                        ""floor""            AS Floor,
                        ""flat_number""      AS FlatNumber,
                        ""full_address""     AS FullAddress,
                        ""created_date""     AS CreatedDate,
                        ""user_id""          AS ApplicationUserId,
                        ""country_id""       AS CountryId,
                        ""city_id""          AS CityId
                    FROM
                        ""addresses""
                    WHERE
                        ""address_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<AddressEntity>(
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

    public async Task<AddressEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(AddressEntity), id);
    }
    
    public async Task<IEnumerable<AddressEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        if (string.IsNullOrEmpty(userId))
            throw new InvalidOperationException();
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "address_id");
            var direction = descending ? "DESC" : "ASC";

            string sql =
                @$"
                    SELECT
                        ""address_id""       AS Id,
                        ""house_code""       AS HouseCode,
                        ""street""           AS Street,
                        ""building_number""  AS BuildingNumber,
                        ""apartment_number"" AS ApartmentNumber,
                        ""entrance""         AS Entrance,
                        ""floor""            AS Floor,
                        ""flat_number""      AS FlatNumber,
                        ""full_address""     AS FullAddress,
                        ""created_date""     AS CreatedDate,
                        ""user_id""          AS ApplicationUserId,
                        ""country_id""       AS CountryId,
                        ""city_id""          AS CityId
                    FROM
                        ""addresses""
                    WHERE 
                        ""user_id"" = @UserId
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<AddressEntity>(sql, new
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

    public async Task<IEnumerable<AddressEntity>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false)
    {
        var result = await this.FindByUserIdAsync(
            userId: userId,
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            page: page,
            pageCount: pageCount,
            descending: descending);

        if (result == null || !result.Any())
            throw new NotFoundException(typeof(AddressEntity), userId);

        return result;
    }
}