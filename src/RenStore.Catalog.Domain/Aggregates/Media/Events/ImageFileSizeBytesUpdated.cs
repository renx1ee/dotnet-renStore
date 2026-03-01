using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageFileSizeBytesUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes)
    : IDomainEvent;