using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");

        RuleFor(p => p.Name)
            .MinimumLength(CatalogConstants.Category.MinCategoryNameLength)
            .MaximumLength(CatalogConstants.Category.MaxCategoryNameLength)
            .WithMessage(
                $"Category name must have lenght between " +
                $"{CatalogConstants.Category.MinCategoryNameLength} and " +
                $"{CatalogConstants.Category.MaxCategoryNameLength}")
            .When(p => !string.IsNullOrEmpty(p.Name));
        
        RuleFor(p => p.NameRu)
            .MinimumLength(CatalogConstants.Category.MinCategoryNameLength)
            .MaximumLength(CatalogConstants.Category.MaxCategoryNameLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                $"Category name ru must have lenght between " +
                $"{CatalogConstants.Category.MinCategoryNameLength} and " +
                $"{CatalogConstants.Category.MaxCategoryNameLength}")
            .When(p => !string.IsNullOrEmpty(p.NameRu));

        RuleFor(x => x.Description)
            .MinimumLength(CatalogConstants.Category.MinDescriptionLength)
            .MaximumLength(CatalogConstants.Category.MaxDescriptionLength)
            .WithMessage(
                $"Category description must have lenght between " +
                $"{CatalogConstants.Category.MinDescriptionLength} and " +
                $"{CatalogConstants.Category.MaxDescriptionLength}")
            .When(p => !string.IsNullOrEmpty(p.Description));
    }
}