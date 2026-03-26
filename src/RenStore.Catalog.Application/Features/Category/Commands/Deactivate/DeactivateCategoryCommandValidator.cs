namespace RenStore.Catalog.Application.Features.Category.Commands.Deactivate;

internal sealed class DeactivateCategoryCommandValidator
    : AbstractValidator<DeactivateCategoryCommand>
{
    public DeactivateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
    }
}