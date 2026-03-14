using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageFileSizeBytesUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes)
    : IDomainEvent;