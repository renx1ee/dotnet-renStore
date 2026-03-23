namespace RenStore.Catalog.Application.Features.Category.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    string? Name = null,
    string? NameRu = null,
    string? Description = null)
    : IRequest;