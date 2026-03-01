using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public record AttributeValueUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Value) 
    : IDomainEvent;