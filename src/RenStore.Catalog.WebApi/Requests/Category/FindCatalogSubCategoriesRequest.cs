namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindCatalogSubCategoriesRequest(
    uint Page,
    uint PageSize, 
    bool Descending,
    SubCategorySortBy SortBy = SubCategorySortBy.Id);