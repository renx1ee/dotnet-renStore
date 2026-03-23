namespace RenStore.Catalog.Application.Features.Category.Commands.Restore;

public sealed record RestoreCategoryCommand(
    Guid CategoryId)
    : IRequest;