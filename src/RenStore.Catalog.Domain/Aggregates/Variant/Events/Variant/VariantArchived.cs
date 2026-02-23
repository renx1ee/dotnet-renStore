using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantArchived(
    DateTimeOffset OccurredAt,
    Guid VariantId)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}