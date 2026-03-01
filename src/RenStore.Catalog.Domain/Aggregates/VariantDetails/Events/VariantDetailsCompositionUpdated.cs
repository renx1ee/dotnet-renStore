using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCompositionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Composition)
    : IDomainEvent;