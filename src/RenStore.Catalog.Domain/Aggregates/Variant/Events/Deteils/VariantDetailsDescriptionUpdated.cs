using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsDescriptionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Description,
    Guid DetailId)
    : IDomainEvent;