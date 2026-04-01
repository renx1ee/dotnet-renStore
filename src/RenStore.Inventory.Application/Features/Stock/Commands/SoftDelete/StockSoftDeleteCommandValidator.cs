namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

internal sealed class StockSoftDeleteCommandValidator
    : AbstractValidator<StockSoftDeleteCommand>
{
    public StockSoftDeleteCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");
    }
}