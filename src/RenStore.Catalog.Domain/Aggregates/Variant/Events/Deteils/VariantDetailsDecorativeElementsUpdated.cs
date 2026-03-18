using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsDecorativeElementsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string DecorativeElements)
    : IDomainEvent;