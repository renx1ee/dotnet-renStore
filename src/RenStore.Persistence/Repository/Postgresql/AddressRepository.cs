using System.ComponentModel;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Criteries;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public interface IAddressRepository
{
    Task<Guid> AddAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task UpdateDetachedUnsafeAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task UpdateRangeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task UpdateDetachedRangeUnsafeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task DeleteHardAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task DeleteRangeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteRangeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken);

    Task DeleteHardRangeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteHardRangeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken);

    Task<int> CommitAsync(
        CancellationToken cancellationToken);
    
    
}

public class AddressRepository : IAddressRepository
{
    // TODO:
    // ❌ _connectionString в репозитории
    // ❌ IConfiguration в конструкторе
    private readonly ILogger<AddressRepository> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private const uint MaxPageSize = 1000;
    private readonly Dictionary<AddressSortBy, string> _sortColumnMapping = new()
    {
        { AddressSortBy.Id, "address_id" },
        { AddressSortBy.HouseCode, "house_code" },
        { AddressSortBy.FlatNumber, "flat_number" }
    };
    
    public AddressRepository(
        ILogger<AddressRepository> logger,
        ApplicationDbContext context,
        string connectionString)
    {
        this._logger           = logger 
                                 ?? throw new ArgumentNullException(nameof(logger));
        this._context          = context 
                                 ?? throw new ArgumentNullException(nameof(context));
        this._connectionString = connectionString  
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public AddressRepository(
        ILogger<AddressRepository> logger,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._logger           = logger 
                                 ?? throw new ArgumentNullException(nameof(logger));
        this._context          = context 
                                 ?? throw new ArgumentNullException(nameof(context));
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new InvalidOperationException(nameof(configuration));
    }

    public async Task<Guid> AddAsync(
        AddressEntity address, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new address.");

        if (address == null)
        {
            this._logger.LogDebug("{Parameter} is null.", nameof(address));
            throw new ArgumentNullException(nameof(address));
        }
        
        address.Id = Guid.NewGuid();
        address.CreatedAt = DateTime.UtcNow;
        
        await this._context.Addresses.AddAsync(address, cancellationToken);
        
        _logger.LogInformation("Address added to context with {AddressId}.", address.Id);
        return address.Id;
    }

    public async Task AddRangeAsync(
        IEnumerable<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        if (addresses == null)
        {
            this._logger.LogDebug("{Parameter} is null.", nameof(addresses));
            throw new ArgumentNullException(nameof(addresses));
        }
        
        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
        {
            this._logger.LogWarning("CreateRangeAsync method called with empty address collection.");
            return;
        }
        
        _logger.LogInformation("Adding {Count} addresses.", addressesList.Count());

        var utcNow = DateTime.UtcNow;
        
        foreach (var address in addressesList)
        {
            address.Id = Guid.NewGuid();
            address.CreatedAt = utcNow;
        }
        await this._context.Addresses.AddRangeAsync(addresses, cancellationToken);
    }
    
    public Task UpdateAsync(
        AddressEntity address, 
        CancellationToken cancellationToken)
    {
        if (address == null)
        {
            this._logger.LogDebug("{Parameter} is null.", nameof(address));
            throw new ArgumentNullException(nameof(address));
        }

        if (address.IsDeleted)
        {
            this._logger.LogWarning("Cannot update IsDeleted entity: {Parameter}.", nameof(address));
            throw new InvalidOperationException("Cannot update address that is marked as deleted.");
        }
        
        this._logger.LogInformation("Update tracked address {AddressId}", address.Id);
        
        address.UpdatedAt = DateTime.UtcNow;
        
        return Task.CompletedTask;
    }
    /// <summary>
    /// Updates a detached entity by marking the entire entity as Modified.
    /// All properties will be updated in the database.
    /// Use ONLY when the entity is fully populated.
    /// </summary>
    public Task UpdateDetachedUnsafeAsync(
        AddressEntity address,
        CancellationToken cancellationToken)
    {
        if (address == null)
        {
            this._logger.LogDebug("{Parameter} is null.", nameof(address));
            throw new ArgumentNullException(nameof(address));
        }
        
        if (address.IsDeleted)
        {
            this._logger.LogWarning("Cannot update IsDeleted entity: {Parameter}.", nameof(address));
            throw new InvalidOperationException("Cannot update address that is marked as deleted.");
        }
        
        this._logger.LogInformation("Update detracked address {AddressId}", address.Id);
        
        address.UpdatedAt = DateTime.UtcNow;

        this._context.Attach(address);
        this._context.Entry(address).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(
        IEnumerable<AddressEntity> addresses, 
        CancellationToken  cancellationToken)
    {
        if (addresses == null)
        {
            this._logger.LogDebug(
                "{Parameter} is null.", nameof(addresses));
            throw new ArgumentNullException(nameof(addresses));
        }

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
        {
            this._logger.LogWarning(
                "UpdateRangeAsync method called with empty address collection.");
            return Task.CompletedTask;
        }

        var deletedCount = addressesList.Count(a => a.IsDeleted);

        if (deletedCount > 0)
        {
            this._logger.LogWarning(
                "UpdateRangeAsync contains {DeletedCount} deleted entities.",
                deletedCount);
            
            throw new InvalidOperationException(
                "Cannot update one or more addresses that is marked as deleted.");
        }
        
        _logger.LogInformation("Updating {Count} tracked addresses.", addressesList.Count);

        var utcNow = DateTime.UtcNow;

        foreach (var address in addressesList)
            address.UpdatedAt = utcNow;
        
        return Task.CompletedTask;
    }
    /// <summary>
    /// This method marks the entire entity as Modified.
    /// All properties will be updated in the database.
    /// Use ONLY when all fields are fully populated.
    /// </summary>
    public Task UpdateDetachedRangeUnsafeAsync(
        IEnumerable<AddressEntity> addresses,
        CancellationToken cancellationToken)
    {
        if (addresses == null)
        {
            this._logger.LogDebug(
                "{Parameter} is null.", nameof(addresses));
            throw new ArgumentNullException(nameof(addresses));
        }

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
        {
            this._logger.LogWarning(
                "UpdateRangeAsync method called with empty address collection.");
            return Task.CompletedTask;
        }

        var deletedCount = addressesList.Count(a => a.IsDeleted);

        if (deletedCount > 0)
        {
            this._logger.LogWarning(
                "UpdateRangeAsync contains {DeletedCount} deleted entities.",
                deletedCount);
            
            throw new InvalidOperationException(
                "Cannot update one or more addresses that is marked as deleted.");
        }
        
        _logger.LogInformation("Updating {Count} detached addresses.", addressesList.Count);

        var utcNow = DateTime.UtcNow;

        foreach (var address in addressesList)
        {
            address.UpdatedAt = utcNow;
            
            this._context.Attach(address);
            this._context.Entry(address).State = EntityState.Modified;
        }
        
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            _logger.LogDebug("Address ID cannot be empty!");
            throw new ArgumentException(
                "Address ID cannot be empty!",
                nameof(id));
        }
        
        var address = await this.GetByIdAsync(id, cancellationToken);

        if (address.IsDeleted)
        {
            this._logger.LogWarning(
                "Attempt to delete already deleted address {AddressId}.", id);
            return;
        }
        
        this._logger.LogInformation("Soft deleting address {AddressId}", id);

        address.IsDeleted = true;
        address.UpdatedAt = DateTime.UtcNow;
    }

    public async Task DeleteHardAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            _logger.LogDebug("Address ID cannot be empty!");
            throw new ArgumentException(
                "Address ID cannot be empty!",
                nameof(id));
        }
        
        var address = await this.GetByIdAsync(id, cancellationToken);
        
        this._logger.LogInformation("Hard deleting address {AddressId}", id);
        
        this._context.Addresses.Remove(address);
    }

    public Task DeleteRangeAsync(
        IEnumerable<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        if (addresses == null)
        {
            _logger.LogDebug("Address cannot be null.");
            throw new ArgumentNullException(nameof(addresses));
        }

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
        {
            _logger.LogWarning("DeleteRangeAsync: called with empty addresses collection");
            return Task.CompletedTask;
        }

        if (addressesList.Any(a => a.Id == Guid.Empty))
        {
            _logger.LogWarning(
                "DeleteRangeAsync: called with empty address IDs collection");
            throw new ArgumentException(
                "One or more address IDs are empty.",
                nameof(addresses));
        }
        
        this._logger.LogInformation("Soft deleting addresses {AddressId}", addressesList);

        var utcNow = DateTime.UtcNow;

        foreach (var address in addresses)
        {
            address.IsDeleted = true;
            address.UpdatedAt = utcNow;
            
            this._context.Attach(address);
            this._context.Entry(address).Property(a => a.IsDeleted).IsModified = true;
            this._context.Entry(address).Property(a => a.UpdatedAt).IsModified = true;
        }
        
        this._logger.LogInformation(
            "Soft deleted {DeletedCount} of {TotalCount} addresses.",
            addressesList.Count(a => a.IsDeleted),
            addressesList.Count);

        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids == null)
        {
            _logger.LogDebug("IDs cannot be null!");
            throw new ArgumentNullException(nameof(ids));
        }

        var idsList = ids as IList<Guid> ?? ids.ToList();

        if (!idsList.Any())
        {
            _logger.LogWarning("DeleteRangeAsync: called with empty addresses collection");
            return Task.CompletedTask;
        }

        if (idsList.Any(id => id == Guid.Empty))
        {
            _logger.LogWarning(
                "DeleteRangeAsync: called with empty address IDs collection");
            throw new ArgumentException(
                "One or more address IDs are empty.",
                nameof(idsList));
        }

        var utcNow = DateTime.UtcNow;

        foreach (var id in idsList)
        {
            var stubAddress = new AddressEntity
            {
                Id = id,
                IsDeleted = true,
                CreatedAt = utcNow
            };

            this._context.Attach(stubAddress);
            this._context.Entry(stubAddress).Property(a => a.IsDeleted).IsModified = true;
            this._context.Entry(stubAddress).Property(a => a.UpdatedAt).IsModified = true;
        }

        this._logger.LogInformation(
            "Soft deleted {Count} addresses (stub addresses)",
            idsList.Count);
        
        return Task.CompletedTask;
    }
    
    public Task DeleteHardRangeAsync(
        IEnumerable<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        if (addresses == null)
        {
            _logger.LogDebug("Address cannot be null.");
            throw new ArgumentNullException(nameof(addresses));
        }

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
        {
            _logger.LogWarning("DeleteHardRangeAsync: called with empty addresses collection");
            return Task.CompletedTask;
        }

        if (addressesList.Any(a => a.Id == Guid.Empty))
        {
            _logger.LogWarning(
                "DeleteHardRangeAsync: called with empty address IDs collection");
            throw new ArgumentException(
                "One or more address IDs are empty.",
                nameof(addresses));
        }
        
        this._logger.LogInformation("Soft deleting addresses {AddressId}", addressesList);

        _context.Addresses.RemoveRange(addressesList);
        
        this._logger.LogInformation(
            "Hard deleted {DeletedCount} of {TotalCount} addresses.",
            addressesList.Count(a => a.IsDeleted),
            addressesList.Count);

        return Task.CompletedTask;
    }
    
    public Task DeleteHardRangeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids == null)
        {
            _logger.LogDebug("IDs cannot be null!");
            throw new ArgumentNullException(nameof(ids));
        }

        var idsList = ids as IList<Guid> ?? ids.ToList();

        if (!idsList.Any())
        {
            _logger.LogWarning("DeleteHardRangeAsync: called with empty addresses collection");
            return Task.CompletedTask;
        }

        if (idsList.Any(id => id == Guid.Empty))
        {
            _logger.LogWarning(
                "DeleteHardRangeAsync: called with empty address IDs collection");
            throw new ArgumentException(
                "One or more address IDs are empty.",
                nameof(idsList));
        }

        var addresses = idsList
            .Select(id => new AddressEntity() { Id = id });

        this._context.RemoveRange(addresses);

        this._logger.LogInformation(
            "Hard deleted {Count} addresses (stub addresses)",
            idsList.Count);
        
        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Commit changes with standalone mode");
        try
        {
            int result = await this._context.SaveChangesAsync(cancellationToken);
            this._logger.LogInformation("Changes commited successfully. Affected rows: {Count}", result);
            return result;
        }
        catch (OperationCanceledException ex)
        {
            this._logger.LogError("Commit operation wac cancelled." );
            throw;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            this._logger.LogError(ex, "Concurrency conflict during commit." );
            throw new ConcurrencyException("Data was notified by another user. Please retry the operation.", ex);
        }
        catch (DbUpdateException ex)
        {
            this._logger.LogError(ex, "Database update failed.");
            throw new DataAccessException("Failed to save changes due to database error.", ex);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Unexpected error during commit.");
            throw;
        }
    }

    public async Task<IReadOnlyList<AddressEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false)
    {
        if (page == 0)
        {
            _logger.LogWarning("Page cannot be 0. Page: {Page}", page);
            throw new ArgumentOutOfRangeException(nameof(page));
        }
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageSize = Math.Min(pageSize, MaxPageSize);
            uint offset = (page - 1) * pageSize;
            var direction = descending ? "DESC" : "ASC";

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            
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
                        ""created_date""     AS CreatedAt,
                        ""updated_date""     AS UpdatedAt,
                        ""is_deleted""       AS IsDeleted,
                        ""user_id""          AS ApplicationUserId,
                        ""country_id""       AS CountryId,
                        ""city_id""          AS CityId
                    FROM
                        ""addresses""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var command = new CommandDefinition(
                commandText: sql, 
                parameters: new
                {   
                    Count = (int)pageSize,
                    Offset = (int)offset
                },
                cancellationToken: cancellationToken);

            var result = await connection
                .QueryAsync<AddressEntity>(command);

            return result.AsList();
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Database error occured.");
            throw new DataException("Database error occured.", e);
        }
    }
    // TODO: сделать по нормальному емае 
    public async Task<IReadOnlyList<AddressEntity>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        bool? includeDeleted = null,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        if (page == 0)
        {
            _logger.LogWarning("Page cannot be 0. Page: {Page}", page);
            throw new ArgumentOutOfRangeException(nameof(page));
        }
        
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageSize = Math.Min(pageSize, MaxPageSize);
            uint offset = (page - 1) * pageSize;
            var direction = descending ? "DESC" : "ASC";
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var sql = new StringBuilder(
                @" 
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
                        ""created_date""     AS CreatedAt,
                        ""updated_date""     AS UpdatedAt,
                        ""is_deleted""       AS IsDeleted,
                        ""user_id""          AS ApplicationUserId,
                        ""country_id""       AS CountryId,
                        ""city_id""          AS CityId
                    FROM
                        ""addresses""
                    WHERE 1 = 1
                ");

            var parameters = new DynamicParameters();
            parameters.Add("Count", pageSize);
            parameters.Add("Offset", offset);

            if (includeDeleted.HasValue)
            {
                sql.Append(" AND \"is_deleted\" = @IsDeleted");
                parameters.Add("IsDeleted", includeDeleted.Value);
            }
            
            if (criteria.CountryId.HasValue)
            {
                sql.Append(" AND \"country_id\" = @CountryId");
                parameters.Add("CountryId", criteria.CountryId);
            }

            if (criteria.CityId.HasValue)
            {
                sql.Append(" AND \"city_id\" = @CityId");
                parameters.Add("CityId", criteria.CityId); 
            }

            sql.Append($" ORDER BY \"{columnName}\" {direction} LIMIT @Count OFFSET @Offset;");

            var commandDefinition = new CommandDefinition(
                commandText: sql.ToString(),
                parameters: parameters,
                cancellationToken: cancellationToken);

            var result = await connection
                .QueryAsync<AddressEntity>(
                    commandDefinition);

            return result.AsList();
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Database error occured.");
            throw;
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
                @"
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
                        ""created_date""     AS CreatedAt,
                        ""updated_date""     AS UpdatedAt,
                        ""is_deleted""       AS IsDeleted,
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

    public async Task<AddressEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
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
            var direction = descending ? "DESC" : "ASC";
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

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
                        ""created_date""     AS CreatedAt,
                        ""updated_date""     AS UpdatedAt,
                        ""is_deleted""       AS IsDeleted,
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
    
    // TODO:
    public async Task<bool> IsExists()
    {
        return false;
    }
}