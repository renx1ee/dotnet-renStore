namespace RenStore.Catalog.Domain.Events.Product;

public record ProductVariantReferenceRemoved(
    Guid ProductId,
    Guid ProductVariantId,
    DateTimeOffset OccurredAt);