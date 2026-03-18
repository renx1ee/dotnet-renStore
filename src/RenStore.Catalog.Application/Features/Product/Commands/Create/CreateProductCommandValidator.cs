namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.SubCategoryId)
            .NotEmpty()
            .WithMessage("Seller ID cannot be Guid empty.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}