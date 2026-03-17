namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByProductId;

internal sealed class FindVariantsByProductIdQueryValidator
    : AbstractValidator<FindVariantsByProductIdQuery>
{
    public FindVariantsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
    }
}