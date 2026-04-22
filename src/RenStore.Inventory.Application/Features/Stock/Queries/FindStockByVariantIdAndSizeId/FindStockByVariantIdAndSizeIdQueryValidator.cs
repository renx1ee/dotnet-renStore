namespace RenStore.Inventory.Application.Features.Stock.Queries.FindStockByVariantIdAndSizeId;

internal sealed class FindStockByVariantIdAndSizeIdQueryValidator
    : AbstractValidator<FindStockByVariantIdAndSizeIdQuery>
{
    public FindStockByVariantIdAndSizeIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");
    }
}