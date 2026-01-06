using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RenStore.Delivery.Application.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.UnitOfWork;

public class DeliveryUnitOfWork
    : IDeliveryUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeliveryUnitOfWork> _logger; 
    
    IAddressQuery AddressesRead { get; }
    ICityQuery CitiesRead { get; }
    ICountryQuery CountriesRead { get; }
    IDeliveryOrderQuery DeliveryOrdersRead { get; }
    IDeliveryTariffQuery DeliveryTariffsRead { get; }
    IDeliveryTrackingQuery DeliveryTrackingsRead { get; }
    IPickupPointQuery PickupPointsRead { get; }
    ISortingCenterQuery SortingCentersRead { get; }

    public DeliveryUnitOfWork(
        ILogger<DeliveryUnitOfWork> logger,
        ApplicationDbContext context)
    {
        this._context = context
                        ?? throw new ArgumentNullException(nameof(context));
        this._logger = logger
                       ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
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

    public void Dispose()
    {
        this._context.Dispose();
    }
}