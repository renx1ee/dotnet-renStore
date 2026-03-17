namespace RenStore.Catalog.Application.Features.Product.Commands.Archive;

internal sealed class ArchiveProductCommandValidator
    : AbstractValidator<ArchiveProductCommand>
{
    public ArchiveProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty guid.");
    }
}