namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record VariantImageRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);