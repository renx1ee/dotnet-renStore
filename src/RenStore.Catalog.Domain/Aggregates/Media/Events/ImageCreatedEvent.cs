using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    Guid VariantId,
    string OriginalFileName,
    string StoragePath,
    long FileSizeBytes,
    bool IsMain,
    int SortOrder,
    int Weight, 
    int Height)
    : IDomainEvent;