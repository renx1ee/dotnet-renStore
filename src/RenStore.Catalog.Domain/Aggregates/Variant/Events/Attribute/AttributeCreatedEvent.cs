using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Key,
    string Value) 
    : IDomainEvent;