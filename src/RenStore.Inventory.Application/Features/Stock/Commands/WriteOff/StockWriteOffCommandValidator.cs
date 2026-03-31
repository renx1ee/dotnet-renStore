namespace RenStore.Inventory.Application.Features.Stock.Commands.WriteOff;

internal sealed class StockWriteOffCommandValidator
    : AbstractValidator<StockWriteOffCommand>
{
    public StockWriteOffCommandValidator()
    {
        RuleFor(x => x.StockId)
            .NotEmpty()
            .WithMessage("Stock ID cannot be empty guid.");

        RuleFor(x => x.Count)
            .GreaterThan(InventoryConstants.VariantStock.MinInventoryStockCount)
            .LessThan(InventoryConstants.VariantStock.MaxInventoryStockCount)
            .WithMessage(
                $"Stock count must be between" +
                $"{InventoryConstants.VariantStock.MinInventoryStockCount} and" +
                $"{InventoryConstants.VariantStock.MaxInventoryStockCount}.");
    }
}