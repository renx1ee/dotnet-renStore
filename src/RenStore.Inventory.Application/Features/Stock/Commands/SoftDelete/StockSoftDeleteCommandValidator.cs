namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

internal sealed class StockSoftDeleteCommandValidator
    : AbstractValidator<StockSoftDeleteCommand>
{
    public StockSoftDeleteCommandValidator()
    {
        RuleFor(x => x.StockId)
            .NotEmpty()
            .WithMessage("Stock ID cannot be empty guid.");
    }
}