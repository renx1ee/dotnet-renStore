namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record VariantImageMainSet(
    DateTimeOffset OccurredAt,
    Guid ImageId);