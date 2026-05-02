using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Tracking.FindTrackingByDeliveryOrderId;

public sealed record FindTrackingByDeliveryOrderIdQuery(Guid DeliveryOrderId)
    : IRequest<IReadOnlyList<DeliveryTrackingReadModel>>;