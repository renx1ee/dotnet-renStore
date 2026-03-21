namespace RenStore.Catalog.Application.Features.Product.Commands.Restore;

internal sealed class RestoreProductCommandValidator
    : AbstractValidator<RestoreProductCommand>
{
    public RestoreProductCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
    }
}