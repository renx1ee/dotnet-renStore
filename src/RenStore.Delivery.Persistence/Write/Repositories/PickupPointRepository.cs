using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class PickupPointRepository
    (ApplicationDbContext context)
    : RenStore.Delivery.Domain.Interfaces.IPickupPointRepository
{
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<long> CreateAsync(
        PickupPoint pickupPoint,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(pickupPoint);

        var result = await this._context.PickupPoints.AddAsync(pickupPoint, cancellationToken);

        return result.Entity.Id;
    }

    public async Task CreateRangeAsync(
        IReadOnlyCollection<PickupPoint> pickupPoints,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(pickupPoints);

        var pickupPointsList = pickupPoints as IList<PickupPoint> ?? pickupPoints.ToList();

        if (pickupPointsList.Count == 0) return;

        await this._context.PickupPoints.AddRangeAsync(pickupPointsList, cancellationToken);
    }

    public void Remove(PickupPoint pickupPoint)
    {
        ArgumentNullException.ThrowIfNull(pickupPoint);

        this._context.PickupPoints.Remove(pickupPoint);
    }
    
    public void RemoveRange(IReadOnlyCollection<PickupPoint> pickupPoints)
    {
        ArgumentNullException.ThrowIfNull(pickupPoints);
        
        this._context.PickupPoints.RemoveRange(pickupPoints);
    }
}