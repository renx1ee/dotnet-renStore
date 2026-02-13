namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record VariantImageMainUnset(
    DateTimeOffset OccurredAt,
    Guid ImageId);