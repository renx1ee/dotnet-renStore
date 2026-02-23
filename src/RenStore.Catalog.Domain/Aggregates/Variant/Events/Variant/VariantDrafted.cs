using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantDrafted(
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}