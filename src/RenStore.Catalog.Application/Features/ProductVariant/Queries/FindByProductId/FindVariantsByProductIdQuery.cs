namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByProductId;

public sealed record FindVariantsByProductIdQuery(
    Guid ProductId,
    ProductVariantSortBy SortBy = ProductVariantSortBy.Id,
    uint Page = 1,
    uint PageCount = 25,
    bool Descending = false,
    bool? IsDeleted = null,
    UserRole? Role = null,
    Guid? UserId = null)
    : IRequest<IReadOnlyList<ProductVariantReadModel>>;