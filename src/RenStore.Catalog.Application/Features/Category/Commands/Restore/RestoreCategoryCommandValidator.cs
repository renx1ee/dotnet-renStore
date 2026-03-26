namespace RenStore.Catalog.Application.Features.Category.Commands.Restore;

internal sealed class RestoreCategoryCommandValidator
    : AbstractValidator<RestoreCategoryCommand>
{
    public RestoreCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
    }
}