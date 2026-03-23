namespace RenStore.Catalog.Application.Features.Category.Commands.DeactivateSubCategory;

public sealed record DeactivateSubCategoryCommand(
    Guid CategoryId,
    Guid SubCategoryId)
    : IRequest;