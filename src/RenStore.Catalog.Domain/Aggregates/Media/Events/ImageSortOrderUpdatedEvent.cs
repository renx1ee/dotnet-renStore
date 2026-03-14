using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageSortOrderUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int SortOrder)
    : IDomainEvent;