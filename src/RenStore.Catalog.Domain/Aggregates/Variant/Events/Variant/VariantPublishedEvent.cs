using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public sealed record VariantPublishedEvent(
    Guid EventId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;