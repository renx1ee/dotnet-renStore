using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsModelFeaturesUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string ModelFeatures)
    : IDomainEvent;