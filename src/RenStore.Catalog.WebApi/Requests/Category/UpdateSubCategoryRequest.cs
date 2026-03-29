namespace RenStore.Catalog.WebApi.Requests.Category;

public sealed record UpdateSubCategoryRequest(
    string? Name = null,
    string? NameRu = null,
    string? Description = null);