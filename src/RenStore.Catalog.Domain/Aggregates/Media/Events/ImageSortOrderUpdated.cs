using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageSortOrderUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    short SortOrder)
    : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}