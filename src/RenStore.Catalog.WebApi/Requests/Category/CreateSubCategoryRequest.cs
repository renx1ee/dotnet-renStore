namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record CreateSubCategoryRequest(
    string Name,
    string NameRu,
    bool IsActive,
    string? Description = null);