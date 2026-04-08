namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record SearchVariantsRequest(
    uint Page,
    uint PageSize, 
    bool Descending, 
    Guid? CategoryId,
    Guid? SubCategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? ColorId,
    string? Search,
    CatalogFilterSortBy SortBy);