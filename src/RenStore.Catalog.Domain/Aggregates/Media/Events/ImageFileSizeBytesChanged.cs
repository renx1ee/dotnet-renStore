namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageFileSizeBytesChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes);