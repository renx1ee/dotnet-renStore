using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface IDeliveryTrackingQuery
{
    Task<IReadOnlyList<DeliveryTrackingReadModel>> FindByDeliveryOrderIdAsync(
        Guid deliveryOrderId,
        CancellationToken cancellationToken);
}