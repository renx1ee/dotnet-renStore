namespace RenStore.Inventory.Application.Features.Stock.Commands.Create;

internal sealed class CreateStockCommandValidator
    : AbstractValidator<CreateStockCommand>
{
    public CreateStockCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size ID cannot be empty guid.");

        RuleFor(x => x.InitialStock)
            .GreaterThan(InventoryConstants.VariantStock.MinInventoryStockCount)
            .LessThan(InventoryConstants.VariantStock.MaxInventoryStockCount)
            .WithMessage(
                $"Initial stock must be between" +
                $"{InventoryConstants.VariantStock.MinInventoryStockCount} and" +
                $"{InventoryConstants.VariantStock.MaxInventoryStockCount}.");
    }
}