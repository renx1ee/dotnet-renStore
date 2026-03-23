namespace RenStore.Catalog.Application.Features.Category.Commands.SoftDelete;

public sealed record SoftDeleteCategoryCommand(
    Guid CategoryId)
    : IRequest;