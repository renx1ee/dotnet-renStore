namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.SubCategoryId)
            .NotEmpty()
            .WithMessage("Sub Category ID cannot be Guid empty.");
        
        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be Guid empty.");
    }
}