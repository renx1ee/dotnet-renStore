namespace RenStore.Catalog.Application.Features.Category.Commands.Activate;

public sealed record ActivateCategoryCommand(
    Guid CategoryId)
    : IRequest;