namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindCatalogCategoriesRequest(
    uint Page,
    uint PageSize, 
    bool Descending,
    CategorySortBy SortBy = CategorySortBy.Id);