using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageDimensionUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height)
    : IDomainEvent;