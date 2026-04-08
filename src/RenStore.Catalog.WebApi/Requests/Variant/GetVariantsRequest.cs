namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record GetVariantsRequest(
    uint Page,
    uint PageSize, 
    bool Descending, 
    Guid? CategoryId,
    Guid? SubCategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? ColorId,
    CatalogFilterSortBy SortBy);