namespace RenStore.Inventory.Application.Features.Stock.Commands.WriteOff;

internal sealed class StockWriteOffCommandHandler
    : IRequestHandler<StockWriteOffCommand>
{
    private readonly ILogger<StockWriteOffCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;

    public StockWriteOffCommandHandler(
        ILogger<StockWriteOffCommandHandler> logger,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(
        StockWriteOffCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Stock: {StockId}",
            nameof(StockWriteOffCommand),
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
        
        stock.SetStock(
            now: DateTimeOffset.UtcNow,
            newStock: request.Count);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(StockWriteOffCommand),
            request.StockId);
    }
}