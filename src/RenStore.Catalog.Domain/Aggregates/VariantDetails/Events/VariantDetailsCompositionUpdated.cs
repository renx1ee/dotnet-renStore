using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCompositionUpdated(
    DateTimeOffset OccurredAt,
    string Composition)
    : IDomainEvent;