namespace RenStore.Catalog.Application.Features.Category.Commands.Deactivate;

public sealed record DeactivateCategoryCommand(
    Guid CategoryId)
    : IRequest;