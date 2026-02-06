namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantImageMainUnset(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);