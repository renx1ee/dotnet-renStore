using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteAttribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveAttribute;

internal sealed class SoftDeleteAttributeFromVariantCommandValidator
    : AbstractValidator<SoftDeleteAttributeFromVariantCommand>
{
    public SoftDeleteAttributeFromVariantCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.AttributeId)
            .NotEmpty()
            .WithMessage("Attribute ID cannot be empty guid.");
    }
}