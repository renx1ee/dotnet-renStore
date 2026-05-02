namespace RenStore.Delivery.Domain.Interfaces;

public interface IDeliveryOrderRepository
{
    Task<Aggregates.DeliveryOrder.DeliveryOrder?> GetAsync(
        Guid              id,
        CancellationToken cancellationToken);

    Task SaveAsync(
        Aggregates.DeliveryOrder.DeliveryOrder order,
        CancellationToken                      cancellationToken);
}