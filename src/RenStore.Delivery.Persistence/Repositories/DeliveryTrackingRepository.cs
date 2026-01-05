using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;

namespace RenStore.Delivery.Persistence.Repositories;

public class DeliveryTrackingRepository
    (ApplicationDbContext context)
    : IDeliveryTrackingRepository
{
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));

    public async Task<Guid> CreateAsync(
        DeliveryTracking tracking,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tracking);

        var result = await this._context.DeliveryTrackings.AddAsync(tracking, cancellationToken);

        return result.Entity.Id;
    }

    public async Task CreateRangeAsync(
        IReadOnlyList<DeliveryTracking> trackings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackings);

        var trackingsList = trackings as IList<DeliveryTracking> ?? trackings.ToList();

        if (trackingsList.Count == 0) return;

        await this._context.DeliveryTrackings.AddRangeAsync(trackingsList, cancellationToken);
    }

    public void Remove(DeliveryTracking tracking)
    {
        ArgumentNullException.ThrowIfNull(tracking);

        this._context.Remove(tracking);
    }
    
    public void RemoveRange(IReadOnlyList<DeliveryTracking> trackings)
    {
        ArgumentNullException.ThrowIfNull(trackings);
        
        this._context.RemoveRange(trackings);
    }
}