namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPage;

internal sealed class FindFullProductPageQueryValidator
    : AbstractValidator<FindFullProductPageQuery>
{
    public FindFullProductPageQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
    }
}