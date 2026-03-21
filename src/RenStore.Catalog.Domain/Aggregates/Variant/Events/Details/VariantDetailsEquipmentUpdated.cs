using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsEquipmentUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Equipment)
    : IDomainEvent;