namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;

internal sealed class AddSizeToVariantCommandValidator
    : AbstractValidator<AddSizeToVariantCommand>
{
    public AddSizeToVariantCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}