namespace RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;

internal sealed class AddToStockCommandValidator
    : AbstractValidator<AddToStockCommand>
{
    public AddToStockCommandValidator()
    {
        RuleFor(x => x.StockId)
            .NotEmpty()
            .WithMessage("Stock ID cannot be empty guid.");
        
        RuleFor(x => x.Count)
            .GreaterThan(InventoryConstants.VariantStock.MinInventoryStockCount)
            .LessThan(InventoryConstants.VariantStock.MaxInventoryStockCount)
            .WithMessage(
                $"Initial stock must be between" +
                $"{InventoryConstants.VariantStock.MinInventoryStockCount} and" +
                $"{InventoryConstants.VariantStock.MaxInventoryStockCount}.");
    }
}