using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageDimensionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height)
    : IDomainEvent;