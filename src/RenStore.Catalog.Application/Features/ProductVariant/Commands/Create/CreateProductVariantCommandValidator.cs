namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

internal sealed class CreateProductVariantCommandValidator
    : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");

        RuleFor(p => p.ColorId)
            .NotEmpty()
            .WithMessage("Color ID cannot be 0");

        RuleFor(p => p.Name)
            .MinimumLength(10)
            .MaximumLength(500)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product variant name cannot be null or empty.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}