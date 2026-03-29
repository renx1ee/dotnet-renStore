namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record CreateCategoryRequest(
    string Name,
    string NameRu,
    bool IsActive,
    string? Description = null);