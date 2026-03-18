using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsModelFeaturesUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string ModelFeatures)
    : IDomainEvent;