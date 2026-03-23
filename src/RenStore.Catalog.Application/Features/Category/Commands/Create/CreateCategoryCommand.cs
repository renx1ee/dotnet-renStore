namespace RenStore.Catalog.Application.Features.Category.Commands.Create;

public sealed record CreateCategoryCommand(
    string Name,
    string NameRu,
    bool IsActive,
    string? Description = null)
    : IRequest<Guid>;