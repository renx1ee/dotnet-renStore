namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategoryById;

internal sealed class FindSubCategoryByIdQueryValidator
    : AbstractValidator<FindSubCategoryByIdQuery>
{
    public FindSubCategoryByIdQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
        
        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage("Sub Category ID cannot be empty guid.");
    }
}