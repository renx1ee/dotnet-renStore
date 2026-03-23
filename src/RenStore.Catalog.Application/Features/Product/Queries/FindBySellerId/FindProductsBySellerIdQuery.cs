namespace RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;

public sealed record FindProductsBySellerIdQuery(
    Guid SellerId,
    ProductSortBy SortBy = ProductSortBy.Id,
    uint Page = 1,
    uint PageCount = 25,
    bool Descending = false,
    bool? IsDeleted = null) 
    : IRequest<IReadOnlyList<ProductReadModel>>;