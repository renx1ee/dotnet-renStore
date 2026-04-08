namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record FindCategoryManageRequest(
    bool? IncludeDeleted = null);