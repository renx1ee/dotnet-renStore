namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindManageCategoriesWithSubCategoriesRequest(
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeletedCategory,
    bool? IsDeletedSubCategory,
    CategorySortBy SortBy = CategorySortBy.Id);