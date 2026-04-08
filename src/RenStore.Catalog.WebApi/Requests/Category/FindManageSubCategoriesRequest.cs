namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindManageSubCategoriesRequest(
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeleted,
    SubCategorySortBy SortBy = SubCategorySortBy.Id);