namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantPublished(
    Guid VariantId,
    DateTimeOffset OccurredAt);