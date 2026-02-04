using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Events.Product;

public record ProductCreated(
    Guid ProductId,
    long SellerId,
    int SubCategoryId,
    ProductStatus Status,
    DateTimeOffset OccurredAt);