namespace RenStore.Inventory.Application.Features.Stock.Commands.ReturnSale;

internal sealed class ReturnStockSaleCommandHandler
    : IRequestHandler<ReturnStockSaleCommand>
{
    private readonly ILogger<ReturnStockSaleCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;

    public ReturnStockSaleCommandHandler(
        ILogger<ReturnStockSaleCommandHandler> logger,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(
        ReturnStockSaleCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with StockId: {StockId}",
            nameof(ReturnStockSaleCommand),
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
        
        stock.ReturnSale(
            now: DateTimeOffset.UtcNow,
            count: request.Count);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(ReturnStockSaleCommand),
            request.StockId);
    }
}