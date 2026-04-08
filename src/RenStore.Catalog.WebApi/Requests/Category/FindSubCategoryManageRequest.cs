namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindSubCategoryManageRequest(
    bool? IncludeDeleted = null);