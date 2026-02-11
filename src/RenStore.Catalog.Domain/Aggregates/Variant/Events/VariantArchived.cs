namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantArchived(
    DateTimeOffset OccurredAt,
    Guid VariantId);