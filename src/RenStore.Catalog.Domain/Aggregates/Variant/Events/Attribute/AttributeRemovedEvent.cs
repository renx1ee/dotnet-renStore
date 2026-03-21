using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public sealed record AttributeRemovedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    Guid AttributeId,
    DateTimeOffset OccurredAt) 
    : IDomainEvent;