using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using DeliveryTrackingSortBy = RenStore.Delivery.Domain.Enums.Sorting.DeliveryTrackingSortBy;

namespace RenStore.Domain.Repository;

public interface IDeliveryTrackingRepository
{
    Task<IEnumerable<DeliveryTracking>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    
    Task<DeliveryTracking?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<DeliveryTracking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<DeliveryTracking>> FindByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<DeliveryTracking>> GetByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}