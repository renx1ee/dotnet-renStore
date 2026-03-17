namespace RenStore.Catalog.Application.Features.Product.Queries.FindById;

internal sealed class FindProductByIdQueryValidator
    : AbstractValidator<FindProductByIdQuery>
{
    public FindProductByIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty Guid.");
    }
}