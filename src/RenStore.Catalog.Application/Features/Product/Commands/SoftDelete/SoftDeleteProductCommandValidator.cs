namespace RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;

internal sealed class SoftDeleteProductCommandValidator
    : AbstractValidator<SoftDeleteProductCommand>
{
    public SoftDeleteProductCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}