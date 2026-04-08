namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategories;

internal sealed class FindSubCategoriesQueryValidator
    : AbstractValidator<FindSubCategoriesQuery>
{
    // TODO:
    public FindSubCategoriesQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty.");
    }
}