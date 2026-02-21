using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageFileSizeBytesUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes)
    : IDomainEvent;