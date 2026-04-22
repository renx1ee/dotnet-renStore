namespace RenStore.Inventory.Application.Features.Stock.Queries.FindByVariantId;

internal sealed class FindStockByVariantIdQueryValidator
    : AbstractValidator<FindStockByVariantIdQuery>
{
    public FindStockByVariantIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}