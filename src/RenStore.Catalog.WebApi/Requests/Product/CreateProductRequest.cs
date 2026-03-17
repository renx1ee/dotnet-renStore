namespace RenStore.Catalog.WebApi.Requests.Product;

public sealed record CreateProductRequest(
    Guid SellerId,
    Guid SubCategoryId);