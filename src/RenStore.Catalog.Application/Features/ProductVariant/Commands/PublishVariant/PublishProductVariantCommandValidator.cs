namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.PublishVariant;

internal sealed class PublishProductVariantCommandValidator
    : AbstractValidator<PublishProductVariantCommand>
{
    public PublishProductVariantCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .Empty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty guid.");
    }
}