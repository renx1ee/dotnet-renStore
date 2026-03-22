using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsDescriptionUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Description,
    Guid DetailId)
    : IDomainEvent;