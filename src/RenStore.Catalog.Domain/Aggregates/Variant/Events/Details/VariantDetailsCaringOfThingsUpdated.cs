using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCaringOfThingsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CaringOfThings)
    : IDomainEvent;