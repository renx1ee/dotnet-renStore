using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCompositionUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid DetailId,
    string Composition)
    : IDomainEvent;