using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using DeliveryOrderSortBy = RenStore.Delivery.Domain.Enums.Sorting.DeliveryOrderSortBy;

namespace RenStore.Domain.Repository;

public interface IDeliveryOrderRepository
{
    Task<Guid> CreateAsync(DeliveryOrder deliveryOrder, CancellationToken cancellationToken);

    Task UpdateAsync(DeliveryOrder deliveryOrder, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<DeliveryOrder>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);

    Task<DeliveryOrder?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<DeliveryOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<DeliveryOrder?> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);

    Task<DeliveryOrder?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
}