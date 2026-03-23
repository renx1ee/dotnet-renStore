namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

internal sealed class SoftDeleteVariantSizeCommandValidator
    : AbstractValidator<SoftDeleteVariantSizeCommand>
{
    public SoftDeleteVariantSizeCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");
    }
}