namespace RenStore.Inventory.Application.Features.Stock.Queries.FindById;

internal sealed class FindStockByIdAsyncQueryValidator
    : AbstractValidator<FindStockByIdAsyncQuery>
{
    public FindStockByIdAsyncQueryValidator()
    {
        RuleFor(x => x.StockId)
            .NotEmpty()
            .WithMessage("Stock ID cannot be empty guid.");
    }   
}