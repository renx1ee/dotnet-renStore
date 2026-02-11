namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

public record VariantImageFileSizeBytesChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    long FileSizeBytes);