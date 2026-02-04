namespace RenStore.Catalog.Domain.Events.Product;

public record ProductVariantReferenceCreated(
    Guid ProductId,
    Guid ProductVariantId,
    DateTimeOffset OccurredAt);