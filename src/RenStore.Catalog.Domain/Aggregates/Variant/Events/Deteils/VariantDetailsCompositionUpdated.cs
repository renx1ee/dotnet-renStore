using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsCompositionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Composition)
    : IDomainEvent;