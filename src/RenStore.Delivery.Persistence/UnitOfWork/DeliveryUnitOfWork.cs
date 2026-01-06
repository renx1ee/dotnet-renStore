using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.UnitOfWork;

public class DeliveryUnitOfWork
    : IDeliveryUnitOfWork, IAsyncDisposable
{
    private readonly ILogger<DeliveryUnitOfWork> _logger;
    private readonly DeliveryDbContext _context;
    
    private IDbContextTransaction? _transaction;

    public DeliveryUnitOfWork(
        ILogger<DeliveryUnitOfWork> logger,
        DeliveryDbContext context)
    {
        this._logger = logger   ?? throw new ArgumentNullException(nameof(logger));
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (this._transaction != null)
            throw new InvalidOperationException("Transaction already started.");
        
        this._transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null) return;
        
        this._logger.LogWarning("Transaction rollback.");
        
        await RollbackInternalAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to commit.");

        try
        {
            var result = await this._context.SaveChangesAsync(cancellationToken);
            await this._transaction!.CommitAsync(cancellationToken);
            return result;
        }
        catch (OperationCanceledException)
        {
            this._logger.LogInformation("Commit operation was cancelled.");
            throw;
        }
        catch (DbUpdateConcurrencyException e)
        {
            this._logger.LogError(e, "Concurrency conflict during commit.");
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
            await RollbackInternalAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task RollbackInternalAsync(CancellationToken cancellationToken)
    {
        if (this._transaction != null)
        {
            await this._transaction.RollbackAsync(cancellationToken);
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (this._transaction != null)
        {
            await this._transaction.DisposeAsync();
            this._transaction = null;
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        await DisposeTransactionAsync();
    }
}