using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantRemoved(
    Guid EventId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;