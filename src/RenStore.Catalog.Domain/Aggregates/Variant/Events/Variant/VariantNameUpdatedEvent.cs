using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public sealed record VariantNameUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId,
    string Name,
    string NormalizedName)
    : IDomainEvent;