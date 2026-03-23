namespace RenStore.Catalog.Application.Features.Category.Commands.UpdateSubCategory;

public sealed record UpdateSubCategoryCommand(
    Guid CategoryId,
    Guid SubCategoryId,
    string? Name = null,
    string? NameRu = null,
    string? Description = null)
    : IRequest;