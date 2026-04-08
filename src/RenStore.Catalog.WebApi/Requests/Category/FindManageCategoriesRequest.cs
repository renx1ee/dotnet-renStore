namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindManageCategoriesRequest(
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeleted,
    CategorySortBy SortBy = CategorySortBy.Id);