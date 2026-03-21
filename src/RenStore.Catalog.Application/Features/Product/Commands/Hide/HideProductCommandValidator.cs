namespace RenStore.Catalog.Application.Features.Product.Commands.Hide;

internal sealed class HideProductCommandValidator
    : AbstractValidator<HideProductCommand>
{
    public HideProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
    }
}