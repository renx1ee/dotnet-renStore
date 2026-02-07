namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantImageRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);