using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCaringOfThingsUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CaringOfThings)
    : IDomainEvent;