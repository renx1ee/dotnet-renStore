using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Commands.CreateSubCategory;

internal sealed class CreateSubCategoryCommandValidator
    : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty guid.");
        
        RuleFor(p => p.Name)
            .MinimumLength(CatalogConstants.Category.MinCategoryNameLength)
            .MaximumLength(CatalogConstants.Category.MaxCategoryNameLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                $"Sub Category name must have lenght between " +
                $"{CatalogConstants.Category.MinCategoryNameLength} and " +
                $"{CatalogConstants.Category.MaxCategoryNameLength}");
        
        RuleFor(p => p.NameRu)
            .MinimumLength(CatalogConstants.Category.MinCategoryNameLength)
            .MaximumLength(CatalogConstants.Category.MaxCategoryNameLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                $"Sub Category name ru must have lenght between " +
                $"{CatalogConstants.Category.MinCategoryNameLength} and " +
                $"{CatalogConstants.Category.MaxCategoryNameLength}");

        RuleFor(x => x.Description)
            .MinimumLength(CatalogConstants.Category.MinDescriptionLength)
            .MaximumLength(CatalogConstants.Category.MaxDescriptionLength)
            .WithMessage(
                $"Sub Category description must have lenght between " +
                $"{CatalogConstants.Category.MinDescriptionLength} and " +
                $"{CatalogConstants.Category.MaxDescriptionLength}")
            .When(p => !string.IsNullOrEmpty(p.Description));
    }
}