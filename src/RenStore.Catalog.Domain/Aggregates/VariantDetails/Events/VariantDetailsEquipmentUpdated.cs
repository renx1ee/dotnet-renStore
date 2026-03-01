using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsEquipmentUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Equipment)
    : IDomainEvent;