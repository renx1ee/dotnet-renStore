namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record UpdateCategoryRequest(
    string? Name = null,
    string? NameRu = null,
    string? Description = null);