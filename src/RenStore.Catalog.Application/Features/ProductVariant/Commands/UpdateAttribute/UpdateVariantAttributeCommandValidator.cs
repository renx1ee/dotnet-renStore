namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateAttribute;

internal sealed class UpdateVariantAttributeCommandValidator
    : AbstractValidator<UpdateVariantAttributeCommand>
{
    public UpdateVariantAttributeCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.AttributeId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(p => p.Key)
            .MinimumLength(10)
            .MaximumLength(500)
            .NotNull()
            .NotEmpty()
            .WithMessage("Attribute key lenght must be between 10 and 500.");
        
        RuleFor(p => p.Value)
            .MinimumLength(10)
            .MaximumLength(500)
            .NotNull()
            .NotEmpty()
            .WithMessage("Attribute value lenght must be between 10 and 500.");
    }
}