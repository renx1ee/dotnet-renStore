namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record VariantImageAdded(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    Guid VariantId);