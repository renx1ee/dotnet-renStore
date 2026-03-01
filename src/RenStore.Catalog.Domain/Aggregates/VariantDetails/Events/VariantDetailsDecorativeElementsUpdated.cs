using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsDecorativeElementsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string DecorativeElements)
    : IDomainEvent;