namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageStoragePathUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    string StoragePath);