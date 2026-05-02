using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderByOrderId;

public sealed record FindDeliveryOrderByOrderIdQuery(Guid OrderId)
    : IRequest<DeliveryOrderReadModel?>;