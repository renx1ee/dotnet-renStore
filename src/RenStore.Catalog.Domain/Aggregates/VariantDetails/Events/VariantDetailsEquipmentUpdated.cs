using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsEquipmentUpdated(
    DateTimeOffset OccurredAt,
    string Equipment)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}