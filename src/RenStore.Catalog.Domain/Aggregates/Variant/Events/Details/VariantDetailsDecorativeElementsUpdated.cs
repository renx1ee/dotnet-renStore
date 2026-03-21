using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsDecorativeElementsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string DecorativeElements)
    : IDomainEvent;