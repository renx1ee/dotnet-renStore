namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveAttribute;

internal sealed class RemoveAttributeFromVariantCommandValidator
    : AbstractValidator<RemoveAttributeFromVariantCommand>
{
    public RemoveAttributeFromVariantCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.AttributeId)
            .NotEmpty()
            .WithMessage("Attribute ID cannot be empty guid.");
    }
}