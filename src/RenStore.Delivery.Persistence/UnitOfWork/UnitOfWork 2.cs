using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.UnitOfWork;

public class UnitOfWork(
    ILogger<UnitOfWork> logger,
    ApplicationDbContext context)
    /*: IUnitOfWork*/
{ 
    private readonly ILogger<UnitOfWork> _logger   = logger 
                                                     ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));
    
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
}