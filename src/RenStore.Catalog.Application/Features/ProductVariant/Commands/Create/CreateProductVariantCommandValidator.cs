using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

internal sealed class CreateProductVariantCommandValidator
    : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");

        RuleFor(p => p.ColorId)
            .NotEmpty()
            .WithMessage("Color ID cannot be 0");

        RuleFor(p => p.Name)
            .MinimumLength(CatalogConstants.ProductVariant.MinProductNameLength)
            .MaximumLength(CatalogConstants.ProductVariant.MaxProductNameLength)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product variant name cannot be null or empty.");
    }
}