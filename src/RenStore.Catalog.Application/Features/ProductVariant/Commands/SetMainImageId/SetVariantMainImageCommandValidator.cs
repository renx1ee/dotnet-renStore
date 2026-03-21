namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;

internal sealed class SetVariantMainImageCommandValidator
    : AbstractValidator<SetVariantMainImageCommand>
{
    public SetVariantMainImageCommandValidator()
    {
        RuleFor(x => x.ImageId)
            .NotEmpty()
            .WithMessage("Image ID cannot be empty Guid.");
        
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty Guid.");
    }
}