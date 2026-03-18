namespace RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;

internal sealed class PublishProductCommandValidator
    : AbstractValidator<PublishProductCommand>
{
    public PublishProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}