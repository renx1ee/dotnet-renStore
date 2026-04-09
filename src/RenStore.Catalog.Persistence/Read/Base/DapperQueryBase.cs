using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace RenStore.Catalog.Persistence.Read.Base;

/// <summary>
/// Base class for read-side query service using Dapper.
/// </summary>
internal abstract class DapperQueryBase(
    CatalogDbContext context,
    ILogger logger)
{
    private const uint MaxPageSize            = 1000;
    protected const int CommandTimeoutSeconds = 30;

    protected readonly ILogger _logger = logger            ?? throw new ArgumentNullException(nameof(logger));
    private readonly CatalogDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Returns current database transaction if exists.
    /// </summary>
    protected DbTransaction? CurrentDbTransaction => 
        _context.Database.CurrentTransaction?.GetDbTransaction();

    protected async Task<DbConnection> GetOpenDbConnectionAsync(
        CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        return connection;
    }

    protected DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occurred.", e);
    }

    /// <summary>
    /// Build pagination parameters for read-side queries.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Throw if page is 0.</exception>
    protected static PageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
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

/// <summary>
/// Represents pagination parameters for read-side queries.
/// Values are expected to be pre-validated by the caller.
/// </summary>
readonly record struct PageRequest(int Limit, int Offset, string Direction);