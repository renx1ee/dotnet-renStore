using RenStore.Catalog.Domain.Constants;

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
            .MinimumLength(CatalogConstants.Attribute.MinKeyLength)
            .MaximumLength(CatalogConstants.Attribute.MaxKeyLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                $"Attribute key lenght must be between " +
                $"{CatalogConstants.Attribute.MinKeyLength} and " +
                $"{CatalogConstants.Attribute.MaxKeyLength}.")
            .When(x => !string.IsNullOrEmpty(x.Key));
        
        RuleFor(p => p.Value)
            .MinimumLength(CatalogConstants.Attribute.MinValueLength)
            .MaximumLength(CatalogConstants.Attribute.MaxValueLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                $"Attribute value lenght must be between " +
                $"{CatalogConstants.Attribute.MinValueLength} and " +
                $"{CatalogConstants.Attribute.MaxValueLength}.")
            .When(x => !string.IsNullOrEmpty(x.Value));
    }
}