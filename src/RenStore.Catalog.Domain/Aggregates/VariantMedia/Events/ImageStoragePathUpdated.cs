namespace RenStore.Catalog.Domain.Aggregates.VariantMedia.Events;

public record ImageStoragePathUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    string StoragePath);