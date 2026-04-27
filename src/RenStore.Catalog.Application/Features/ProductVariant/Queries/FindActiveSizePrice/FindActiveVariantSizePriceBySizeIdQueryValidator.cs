namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindActiveSizePrice;

internal sealed class FindActiveVariantSizePriceBySizeIdQueryValidator
    : AbstractValidator<FindActiveVariantSizePriceBySizeIdQuery>
{
    public FindActiveVariantSizePriceBySizeIdQueryValidator()
    {
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty Guid");
        
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty Guid");
    }
}