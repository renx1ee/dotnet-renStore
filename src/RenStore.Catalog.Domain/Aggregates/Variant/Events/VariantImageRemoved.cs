namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantImageRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);