namespace RenStore.Catalog.Application.Features.Category.Commands.SoftDeleteSubCategory;

public sealed record SoftDeleteSubCategoryCommand(
    Guid CategoryId,
    Guid SubCategoryId)
    : IRequest;