namespace RenStore.Catalog.WebApi.Requests;

public sealed record CreateProductRequest(
    long SellerId,
    Guid SubCategoryId);