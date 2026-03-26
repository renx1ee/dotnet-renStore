namespace RenStore.Catalog.Application.Features.Category.Commands.SoftDelete;

internal sealed class SoftDeleteCategoryCommandValidator
    : AbstractValidator<SoftDeleteCategoryCommand>
{
    public SoftDeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
    }
}