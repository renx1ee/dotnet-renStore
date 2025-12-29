using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IDeliveryTrackingRepository
{
    Task<IEnumerable<DeliveryTrackingEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    
    Task<DeliveryTrackingEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<DeliveryTrackingEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<DeliveryTrackingEntity>> FindByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<DeliveryTrackingEntity>> GetByDeliveryOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}