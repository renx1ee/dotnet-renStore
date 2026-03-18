using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsCaringOfThingsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CaringOfThings)
    : IDomainEvent;