using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageMainSetEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId)
    : IDomainEvent;