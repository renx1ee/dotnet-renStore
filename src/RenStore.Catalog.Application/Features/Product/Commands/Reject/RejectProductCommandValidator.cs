namespace RenStore.Catalog.Application.Features.Product.Commands.Reject;

internal sealed class RejectProductCommandValidator
    : AbstractValidator<RejectProductCommand>
{
    public RejectProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
    }
}