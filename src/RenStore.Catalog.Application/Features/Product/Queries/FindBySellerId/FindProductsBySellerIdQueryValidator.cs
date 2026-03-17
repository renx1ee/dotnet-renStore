namespace RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;

internal sealed class FindProductsBySellerIdQueryValidator
    : AbstractValidator<FindProductsBySellerIdQuery>
{
    public FindProductsBySellerIdQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty()
            .WithMessage("Seller ID cannot be empty guid.");
    }
}