namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindPriceHistoryBySizeId;

internal sealed class FindPriceHistoryBySizeIdQueryValidator
    : AbstractValidator<FindPriceHistoryBySizeIdQuery>
{
    public FindPriceHistoryBySizeIdQueryValidator()
    {
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty Guid");
        
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty Guid");
    }
}