namespace RenStore.Order.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Domain.Aggregates.Order.Order?> GetAsync(
        Guid orderId,
        CancellationToken cancellationToken);

    Task SaveAsync(
        Domain.Aggregates.Order.Order order,
        CancellationToken cancellationToken);
}