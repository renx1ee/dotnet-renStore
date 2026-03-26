namespace RenStore.Catalog.Application.Features.Category.Commands.RestoreSubCategory;

internal sealed class RestoreSubCategoryCommandValidator
    : AbstractValidator<RestoreSubCategoryCommand>
{
    public RestoreSubCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Sub Category ID cannot be empty guid.");
    }
}