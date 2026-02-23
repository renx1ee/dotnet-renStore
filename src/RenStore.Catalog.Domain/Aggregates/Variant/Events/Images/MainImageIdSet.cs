using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record MainImageIdSet(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}