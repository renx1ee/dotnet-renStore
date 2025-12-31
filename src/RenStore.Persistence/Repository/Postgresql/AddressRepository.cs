using System.Data;
using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Application.Abstractions.Repository;
using RenStore.Application.Features.Address.Queries;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;
/// <remarks>
/// This repository supports optional ambient EF Core transactions.
/// Dapper queries participate in the current transaction if present, 
/// but will not see uncommitted EF Core changes until SaveChanges is called.
/// </remarks>
internal sealed class AddressRepository(
    ILogger<AddressRepository> logger,
    ApplicationDbContext context)
    : IAddressRepository
{
    private const uint MaxPageSize = 1000;
    private const int CommandTimeoutSeconds = 30;
    
    private const string BaseSqlQuery = 
        """
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
                "addresses"
        """;
    
    private static readonly Dictionary<AddressSortBy, string> _sortColumnMapping = new()
    {
        { AddressSortBy.Id, "address_id" },
        { AddressSortBy.HouseCode, "house_code" },
        { AddressSortBy.FlatNumber, "flat_number" }
    };
    
    private readonly ILogger<AddressRepository> _logger = logger 
                                                          ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context      = context 
                                                          ?? throw new ArgumentNullException(nameof(context));

    private DbTransaction? CurrentTransaction => 
        this._context.Database.CurrentTransaction?.GetDbTransaction();
    
    public async Task<Guid> AddAsync(
        AddressEntity address, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);
        
        address.Id = Guid.NewGuid();
        address.CreatedAt = DateTime.UtcNow;
        
        await this._context.Addresses.AddAsync(address, cancellationToken);
        
        return address.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);
        
        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any()) return;

        var utcNow = DateTime.UtcNow;
        
        foreach (var address in addressesList)
        {
            address.Id = Guid.NewGuid();
            address.CreatedAt = utcNow;
        }
        
        await this._context.Addresses.AddRangeAsync(addressesList, cancellationToken);
    }
    
    public Task UpdateAsync(
        AddressEntity address, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.IsDeleted)
            throw new InvalidOperationException("Cannot update address that is marked as deleted.");
        
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
        ArgumentNullException.ThrowIfNull(address);
        
        if (address.IsDeleted)
            throw new InvalidOperationException("Cannot update address that is marked as deleted.");
        
        address.UpdatedAt = DateTime.UtcNow;

        this._context.Attach(address);
        this._context.Entry(address).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses, 
        CancellationToken  cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
            return Task.CompletedTask;
        
        var deletedCount = addressesList.Count(a => a.IsDeleted);

        if (deletedCount > 0)
            throw new InvalidOperationException(
                "Cannot update one or more addresses that is marked as deleted.");

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
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
            return Task.CompletedTask;
        

        var deletedCount = addressesList.Count(a => a.IsDeleted);

        if (deletedCount > 0)
            throw new InvalidOperationException(
                "Cannot update one or more addresses that is marked as deleted.");

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
            throw new ArgumentException("Address ID cannot be empty!", nameof(id));
        
        var address = await this.GetByIdAsync(id, cancellationToken);

        if (address.IsDeleted) return;

        address.IsDeleted = true;
        address.UpdatedAt = DateTime.UtcNow;
    }

    public async Task DeleteHardAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Address ID cannot be empty!", nameof(id));
        
        var address = await this.GetByIdAsync(id, cancellationToken);
        
        this._context.Addresses.Remove(address);
    }

    public Task DeleteRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);

        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
            return Task.CompletedTask;

        if (addressesList.Any(a => a.Id == Guid.Empty))
            throw new ArgumentException("One or more address IDs are empty.", nameof(addresses));

        var utcNow = DateTime.UtcNow;

        foreach (var address in addresses)
        {
            address.IsDeleted = true;
            address.UpdatedAt = utcNow;
            
            this._context.Attach(address);
            this._context.Entry(address).Property(a => a.IsDeleted).IsModified = true;
            this._context.Entry(address).Property(a => a.UpdatedAt).IsModified = true;
        }

        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var idsList = ids as IList<Guid> ?? ids.ToList();

        if (!idsList.Any())
            return Task.CompletedTask;

        if (idsList.Any(id => id == Guid.Empty))
            throw new ArgumentException("One or more address IDs are empty.", nameof(idsList));

        var utcNow = DateTime.UtcNow;

        foreach (var id in idsList)
        {
            var stubAddress = new AddressEntity
            {
                Id = id,
                IsDeleted = true,
                UpdatedAt = utcNow
            };

            this._context.Attach(stubAddress);
            this._context.Entry(stubAddress).Property(a => a.IsDeleted).IsModified = true;
            this._context.Entry(stubAddress).Property(a => a.UpdatedAt).IsModified = true;
        }
        
        return Task.CompletedTask;
    }
    
    public Task DeleteHardRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);
        
        var addressesList = addresses as IList<AddressEntity> ?? addresses.ToList();

        if (!addressesList.Any())
            return Task.CompletedTask;

        if (addressesList.Any(a => a.Id == Guid.Empty))
            throw new ArgumentException("One or more address IDs are empty.", nameof(addresses));

        _context.Addresses.RemoveRange(addressesList);

        return Task.CompletedTask;
    }
    
    public Task DeleteHardRangeAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var idsList = ids as IList<Guid> ?? ids.ToList();

        if (!idsList.Any())
            return Task.CompletedTask;

        if (idsList.Any(id => id == Guid.Empty))
            throw new ArgumentException("One or more address IDs are empty.", nameof(idsList));

        var addresses = idsList
            .Select(id => new AddressEntity() { Id = id });

        this._context.RemoveRange(addresses);
        
        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            int result = await this._context.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (OperationCanceledException)
        {
            this._logger.LogInformation("Commit operation was cancelled." );
            throw;
        }
        catch (DbUpdateConcurrencyException e)
        {
            this._logger.LogError(e, "Concurrency conflict during commit." );
            throw new ConcurrencyException("Data was notified by another user. Please retry the operation.", e);
        }
        catch (DbUpdateException e)
        {
            this._logger.LogError(e, "Database update failed.");
            throw new DataAccessException("Failed to save changes due to database error.", e);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Unexpected error during commit.");
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
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            string sql = 
                @$"
                    {BaseSqlQuery}
                    ORDER BY ""{columnName}"" {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<AddressEntity>(
                    new CommandDefinition(
                        commandText: sql, 
                        parameters: new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset
                        },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<IReadOnlyList<AddressEntity>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        bool? includeDeleted = null,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var sql = new StringBuilder(
                $@" 
                    {BaseSqlQuery}
                    WHERE 1 = 1
                ");

            var parameters = new DynamicParameters();
            parameters.Add("Count", pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

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
            
            if (!string.IsNullOrEmpty(criteria.UserId))
            {
                sql.Append(" AND \"user_id\" = @UserId");
                parameters.Add("UserId", criteria.UserId); 
            }
            
            if (!string.IsNullOrEmpty(criteria.Street))
            {
                sql.Append(" AND \"street\" = @Street");
                parameters.Add("Street", criteria.Street); 
            }
            
            if (!string.IsNullOrEmpty(criteria.BuildingNumber))
            {
                sql.Append(" AND \"building_number\" = @BuildingNumber");
                parameters.Add("BuildingNumber", criteria.BuildingNumber); 
            }
            
            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction} 
                           LIMIT @Count OFFSET @Offset;");

            var result = await connection
                .QueryAsync<AddressEntity>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: parameters,
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<AddressEntity?> FindByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            string sql =
                $@"
                    {BaseSqlQuery}
                    WHERE
                        ""address_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<AddressEntity>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters: new { Id = id },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<AddressEntity> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(AddressEntity), id);
    }
    
    public async Task<IReadOnlyList<AddressEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("UserId cannot be empty", nameof(userId));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);

            var pageRequest = BuildPageRequest(page, pageSize, descending);
            
            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
            {
                _logger.LogWarning("{Argument} out of range.", sortBy);
                throw new ArgumentOutOfRangeException(nameof(sortBy));
            }
            
            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE 
                        ""user_id"" = @UserId
                    ORDER BY ""{columnName}"" {pageRequest.Direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            var result = await connection
                .QueryAsync<AddressEntity>(
                    new CommandDefinition(
                        commandText: sql,
                        parameters:new
                        {   
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            UserId = userId
                        },
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<bool> IsExists(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("AddressId cannot be empty!", nameof(id));
        
        try
        {
            var connection = await this.GetOpenConnectionAsync(cancellationToken);
            
            string sql = 
                @"
                    SELECT EXISTS (
                        SELECT 1
                        FROM 
                            ""addresses""
                        WHERE 
                            ""address_id"" = @Id
                    );
                ";

            var result = await connection
                .ExecuteScalarAsync<bool>(
                    new CommandDefinition(
                        parameters: new { Id = id},
                        commandText: sql,
                        transaction: CurrentTransaction,
                        commandTimeout: CommandTimeoutSeconds, 
                        cancellationToken: cancellationToken));

            return result;
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    private async Task<DbConnection> GetOpenConnectionAsync(
        CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
            
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }

    private DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occured.", e);
    }
    
    private static PageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
    {
        if (page == 0) 
            throw new ArgumentOutOfRangeException(nameof(page));
        
        pageSize = Math.Min(pageSize, MaxPageSize);
        uint offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";

        return new PageRequest(
            Limit: (int)pageSize,
            Offset: (int)offset,
            Direction: direction);
    }
}

readonly record struct PageRequest(int Limit, int Offset, string Direction);