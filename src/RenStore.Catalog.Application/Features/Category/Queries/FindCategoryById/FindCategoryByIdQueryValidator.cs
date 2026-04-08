namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategoryById;

internal sealed class FindCategoryByIdQueryValidator
    : AbstractValidator<FindCategoryByIdQuery>
{
    public FindCategoryByIdQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
    }
}