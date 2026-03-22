using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsEquipmentUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Equipment,
    Guid DetailId)
    : IDomainEvent;