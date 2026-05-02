using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Projections;

public interface IDeliveryTrackingProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(
        DeliveryTrackingReadModel tracking,
        CancellationToken         cancellationToken);
}