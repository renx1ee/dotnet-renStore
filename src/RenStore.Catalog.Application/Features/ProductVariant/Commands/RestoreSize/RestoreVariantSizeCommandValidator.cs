namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

internal sealed class RestoreVariantSizeCommandValidator
    : AbstractValidator<RestoreVariantSizeCommand>
{
    public RestoreVariantSizeCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");
    }
}