namespace RenStore.Catalog.Domain.Aggregates.VariantMedia.Events;

public record ImageFileSizeBytesChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes);