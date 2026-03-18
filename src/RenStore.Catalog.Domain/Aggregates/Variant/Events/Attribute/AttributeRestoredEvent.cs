using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeRestoredEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId) 
    : IDomainEvent;