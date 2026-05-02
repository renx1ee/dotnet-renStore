using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.Write.Projections;

internal sealed class DeliveryTrackingProjection(
    DeliveryDbContext context)
    : IDeliveryTrackingProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(
        DeliveryTrackingReadModel tracking,
        CancellationToken         cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tracking);
        await context.DeliveryTrackings.AddAsync(tracking, cancellationToken);
    }
}