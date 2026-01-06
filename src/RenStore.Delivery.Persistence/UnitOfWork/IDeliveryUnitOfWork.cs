using RenStore.Delivery.Application.Interfaces;

namespace RenStore.Delivery.Persistence.UnitOfWork;

public interface IDeliveryUnitOfWork 
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}