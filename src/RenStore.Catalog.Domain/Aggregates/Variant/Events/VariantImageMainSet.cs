namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantImageMainSet(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);