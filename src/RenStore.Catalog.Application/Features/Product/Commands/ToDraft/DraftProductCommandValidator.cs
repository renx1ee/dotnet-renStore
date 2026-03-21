namespace RenStore.Catalog.Application.Features.Product.Commands.ToDraft;

internal sealed class DraftProductCommandValidator
    : AbstractValidator<DraftProductCommand>
{
    public DraftProductCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid");
    }
}