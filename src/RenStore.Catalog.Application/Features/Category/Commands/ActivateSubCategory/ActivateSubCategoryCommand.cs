namespace RenStore.Catalog.Application.Features.Category.Commands.ActivateSubCategory;

public sealed record ActivateSubCategoryCommand(
    Guid CategoryId,
    Guid SubCategoryId)
    : IRequest;