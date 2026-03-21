using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCompositionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Composition)
    : IDomainEvent;