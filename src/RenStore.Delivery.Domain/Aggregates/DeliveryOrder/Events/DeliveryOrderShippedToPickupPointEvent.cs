using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;

public sealed record DeliveryOrderShippedToPickupPointEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           DeliveryOrderId,
    long           PickupPointId) 
    : IDomainEvent;