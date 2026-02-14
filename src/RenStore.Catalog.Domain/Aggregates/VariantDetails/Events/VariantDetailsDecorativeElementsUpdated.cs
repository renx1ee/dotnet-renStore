namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsDecorativeElementsUpdated(
    DateTimeOffset OccurredAt,
    string DecorativeElements,
    Guid VariantId);