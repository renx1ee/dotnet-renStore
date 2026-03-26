using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

internal sealed class ChangeProductVariantNameCommandValidator
    : AbstractValidator<ChangeProductVariantNameCommand>
{
    public ChangeProductVariantNameCommandValidator()
    {
        RuleFor(v => v.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(p => p.Name)
            .MinimumLength(CatalogConstants.ProductVariant.MinProductNameLength)
            .MaximumLength(CatalogConstants.ProductVariant.MaxProductNameLength)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product variant name cannot be null or empty.");
    }
}