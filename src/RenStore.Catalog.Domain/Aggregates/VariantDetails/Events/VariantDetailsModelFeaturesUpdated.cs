using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsModelFeaturesUpdated(
    DateTimeOffset OccurredAt,
    string ModelFeatures)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}