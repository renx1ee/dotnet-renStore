namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.SearchVariants;

public sealed record SearchVariantsQuery(
    uint Page,
    uint PageSize, 
    bool Descending, 
    Guid? CategoryId,
    Guid? SubCategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? ColorId,
    string? Search,
    CatalogFilterSortBy SortBy)
    : IRequest<IReadOnlyList<CatalogReadModel>>;