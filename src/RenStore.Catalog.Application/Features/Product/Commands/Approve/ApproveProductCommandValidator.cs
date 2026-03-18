namespace RenStore.Catalog.Application.Features.Product.Commands.Approve;

internal sealed class ApproveProductCommandValidator
    : AbstractValidator<ApproveProductCommand>
{
    public ApproveProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}