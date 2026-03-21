using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsModelFeaturesUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string ModelFeatures)
    : IDomainEvent;