using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageSortOrderUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    short SortOrder)
    : IDomainEvent;