using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeValueUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Value) 
    : IDomainEvent;