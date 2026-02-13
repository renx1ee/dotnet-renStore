namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record VariantImageRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);