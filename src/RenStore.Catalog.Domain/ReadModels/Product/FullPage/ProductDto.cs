using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record ProductDto(
    Guid ProductId,
    string Status,
    Guid SellerId,
    Guid CategoryId,
    Guid SubCategoryId);