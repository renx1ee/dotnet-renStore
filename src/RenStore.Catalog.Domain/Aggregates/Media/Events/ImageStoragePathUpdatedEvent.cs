using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageStoragePathUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    string StoragePath)
    : IDomainEvent;