namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;

internal sealed class AddPriceToVariantSizeCommandValidator
    : AbstractValidator<AddPriceToVariantSizeCommand>
{
    public AddPriceToVariantSizeCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");

        RuleFor(s => s.Price)
            .GreaterThan(0)
            .WithMessage("Price cannot be ");
    }
}