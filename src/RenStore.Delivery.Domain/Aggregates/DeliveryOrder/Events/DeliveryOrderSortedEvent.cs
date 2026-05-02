using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;

public sealed record DeliveryOrderSortedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           DeliveryOrderId,
    long           SortingCenterId) 
    : IDomainEvent;