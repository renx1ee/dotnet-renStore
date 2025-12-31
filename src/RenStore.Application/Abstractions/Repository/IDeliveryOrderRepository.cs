using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IDeliveryOrderRepository
{
    Task<Guid> CreateAsync(DeliveryOrderEntity deliveryOrder, CancellationToken cancellationToken);

    Task UpdateAsync(DeliveryOrderEntity deliveryOrder, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<DeliveryOrderEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);

    Task<DeliveryOrderEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<DeliveryOrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<DeliveryOrderEntity?> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);

    Task<DeliveryOrderEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
}