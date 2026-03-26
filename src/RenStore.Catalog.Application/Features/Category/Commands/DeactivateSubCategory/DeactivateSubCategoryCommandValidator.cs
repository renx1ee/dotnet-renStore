namespace RenStore.Catalog.Application.Features.Category.Commands.DeactivateSubCategory;

internal sealed class DeactivateCategoryCommandValidator
    : AbstractValidator<DeactivateSubCategoryCommand>
{
    public DeactivateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
        
        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage("Sub Category ID cannot be empty guid.");
    }
}