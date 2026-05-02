using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;

public sealed record DeliveryOrderCreatedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           DeliveryOrderId,
    Guid           OrderId,
    int            DeliveryTariffId) 
    : IDomainEvent;