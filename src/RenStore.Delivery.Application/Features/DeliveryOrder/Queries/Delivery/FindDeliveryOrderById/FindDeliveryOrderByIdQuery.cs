using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderById;

public sealed record FindDeliveryOrderByIdQuery(Guid DeliveryOrderId)
    : IRequest<DeliveryOrderReadModel?>;