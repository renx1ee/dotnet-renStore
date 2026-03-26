namespace RenStore.Catalog.Application.Features.Category.Commands.Activate;

internal sealed class ActivateCategoryCommandValidator
    : AbstractValidator<ActivateCategoryCommand>
{
    public ActivateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
    }
}