using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeRemovedEvent(
    Guid EventId,
    Guid AttributeId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;