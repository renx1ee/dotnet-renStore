using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCaringOfThingsUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CaringOfThings,
    Guid DetailId)
    : IDomainEvent;