using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsDecorativeElementsUpdated(
    DateTimeOffset OccurredAt,
    string DecorativeElements)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}