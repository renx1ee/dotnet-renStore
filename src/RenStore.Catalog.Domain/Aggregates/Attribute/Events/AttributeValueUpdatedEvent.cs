using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public sealed record AttributeValueUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Value) 
    : IDomainEvent;