using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;

public sealed record DeliveryOrderShippedToSortingCenterEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           DeliveryOrderId,
    long           DestinationSortingCenterId) 
    : IDomainEvent;