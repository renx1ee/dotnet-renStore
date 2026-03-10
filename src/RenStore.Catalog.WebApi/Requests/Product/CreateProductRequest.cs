namespace RenStore.Catalog.WebApi.Requests.Product;

public sealed record CreateProductRequest(
    long SellerId,
    Guid SubCategoryId);