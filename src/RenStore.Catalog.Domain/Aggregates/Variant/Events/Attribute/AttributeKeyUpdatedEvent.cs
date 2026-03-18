using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeKeyUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Key) 
    : IDomainEvent;