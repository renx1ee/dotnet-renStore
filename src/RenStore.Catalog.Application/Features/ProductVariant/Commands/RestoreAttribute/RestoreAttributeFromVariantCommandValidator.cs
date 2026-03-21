namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreAttribute;

internal sealed class RestoreAttributeFromVariantCommandValidator
    : AbstractValidator<RestoreAttributeFromVariantCommand>
{
    public RestoreAttributeFromVariantCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.AttributeId)
            .NotEmpty()
            .WithMessage("Attribute ID cannot be empty guid.");
    }
}