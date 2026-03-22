using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsModelFeaturesUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string ModelFeatures,
    Guid DetailId)
    : IDomainEvent;