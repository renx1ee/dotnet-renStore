using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class DeliveryOrderRepository
    (ApplicationDbContext context)
    : RenStore.Delivery.Domain.Interfaces.IDeliveryOrderRepository
{
    private readonly ApplicationDbContext _context = context
                                                     ?? throw new ArgumentNullException(nameof(context));

    public async Task<Guid> AddAsync(
        DeliveryOrder deliveryOrder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(deliveryOrder);

        var result = await this._context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);

        return result.Entity.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<DeliveryOrder> deliveryOrders,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(deliveryOrders);

        var deliveriesList = deliveryOrders as IList<DeliveryOrder> ?? deliveryOrders.ToList();

        if (deliveriesList.Count == 0) return;

        await this._context.DeliveryOrders.AddRangeAsync(deliveriesList, cancellationToken);
    }

    public void Remove(DeliveryOrder deliveryOrder)
    {
        ArgumentNullException.ThrowIfNull(deliveryOrder);

        this._context.DeliveryOrders.Remove(deliveryOrder);
    }
    
    public void RemoveRange(IReadOnlyCollection<DeliveryOrder> deliveryOrderies)
    {
        ArgumentNullException.ThrowIfNull(deliveryOrderies);

        this._context.DeliveryOrders.RemoveRange(deliveryOrderies);
    }
}