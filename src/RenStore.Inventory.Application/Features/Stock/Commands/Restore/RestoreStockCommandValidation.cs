namespace RenStore.Inventory.Application.Features.Stock.Commands.Restore;

internal sealed class RestoreStockCommandValidation
    : AbstractValidator<RestoreStockCommand>
{
    public RestoreStockCommandValidation()
    {
        RuleFor(x => x.StockId)
            .NotEmpty()
            .WithMessage("Stock ID cannot be empty guid.");
    }
}