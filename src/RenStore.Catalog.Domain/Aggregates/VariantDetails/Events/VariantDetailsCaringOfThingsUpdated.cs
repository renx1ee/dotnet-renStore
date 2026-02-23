using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCaringOfThingsUpdated(
    DateTimeOffset OccurredAt,
    string CaringOfThings)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}