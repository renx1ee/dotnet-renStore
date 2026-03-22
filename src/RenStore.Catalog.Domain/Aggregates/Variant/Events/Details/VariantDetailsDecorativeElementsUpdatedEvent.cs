using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsDecorativeElementsUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string DecorativeElements,
    Guid DetailId)
    : IDomainEvent;