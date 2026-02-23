using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageDimensionUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}