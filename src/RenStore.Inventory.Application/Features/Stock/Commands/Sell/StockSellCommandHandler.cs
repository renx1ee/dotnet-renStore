namespace RenStore.Inventory.Application.Features.Stock.Commands.Sell;

internal sealed class StockSellCommandHandler
    : IRequestHandler<StockSellCommand>
{
    private readonly ILogger<StockSellCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;

    public StockSellCommandHandler(
        ILogger<StockSellCommandHandler> logger,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(
        StockSellCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Stock: {StockId}",
            nameof(StockSellCommand),
            request.StockId);

        var stock = await _stockRepository.GetAsync(
            stockId: request.StockId,
            cancellationToken: cancellationToken);

        if (stock is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStock),
                request.StockId);
        }
        
        stock.Sell(
            now: DateTimeOffset.UtcNow,
            count: request.Count);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(StockSellCommand),
            request.StockId);
    }
}