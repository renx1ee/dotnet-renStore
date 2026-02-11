namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

public record VariantImageStoragePathUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    string StoragePath);