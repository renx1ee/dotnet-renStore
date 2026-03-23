namespace RenStore.Catalog.Application.Features.Category.Commands.RestoreSubCategory;

public sealed record RestoreSubCategoryCommand(
    Guid CategoryId,
    Guid SubCategoryId)
    : IRequest;