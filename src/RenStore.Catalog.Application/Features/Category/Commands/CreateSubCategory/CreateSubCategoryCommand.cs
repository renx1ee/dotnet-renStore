namespace RenStore.Catalog.Application.Features.Category.Commands.CreateSubCategory;

public sealed record CreateSubCategoryCommand(
    Guid CategoryId,
    string Name,
    string NameRu,
    bool IsActive,
    string? Description = null)
    : IRequest<Guid>;