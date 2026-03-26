namespace RenStore.Catalog.Application.Features.Category.Commands.ActivateSubCategory;

internal sealed class ActivateSubCategoryCommandValidator
    : AbstractValidator<ActivateSubCategoryCommand>
{
    public ActivateSubCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
        
        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage("Sub Category ID cannot be empty guid.");
    }
}