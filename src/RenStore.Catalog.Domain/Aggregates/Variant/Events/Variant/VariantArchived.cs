using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantArchived(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId)
    : IDomainEvent;