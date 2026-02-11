namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsDecorativeElementsUpdated(
    DateTimeOffset OccurredAt,
    string DecorativeElements,
    Guid VariantId);