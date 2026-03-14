using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public sealed record AttributeKeyUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Key) 
    : IDomainEvent;