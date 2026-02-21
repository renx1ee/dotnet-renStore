using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public record AttributeKeyUpdated(
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Key) 
    : IDomainEvent;