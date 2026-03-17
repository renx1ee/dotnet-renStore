namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.SellerId)
            .NotEmpty()
            .WithMessage("Seller ID cannot be empty Guid.");
        
        RuleFor(p => p.SubCategoryId)
            .NotEmpty()
            .WithMessage("Seller ID cannot be Guid empty.");
    }
}