namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByVariantId;

internal sealed class FindFullProductPageByVariantIdQueryValidator
    : AbstractValidator<FindFullProductPageByVariantIdQuery>
{
    public FindFullProductPageByVariantIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}