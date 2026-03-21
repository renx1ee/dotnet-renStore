using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantRestoredEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;