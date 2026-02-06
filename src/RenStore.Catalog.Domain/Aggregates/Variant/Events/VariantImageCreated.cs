namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantImageCreated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    Guid VariantId,
    string OriginalFileName,
    string StoragePath,
    long FileSizeBytes,
    bool IsMain,
    short SortOrder,
    int Weight, 
    int Height);