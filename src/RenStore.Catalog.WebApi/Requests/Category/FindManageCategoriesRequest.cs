namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindManageCategoriesRequest(
    uint Page = 1,
    uint PageSize = 20, 
    bool Descending = true,
    bool? IsDeleted = null,
    CategorySortBy SortBy = CategorySortBy.Id);