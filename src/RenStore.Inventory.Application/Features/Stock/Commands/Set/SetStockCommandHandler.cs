namespace RenStore.Inventory.Application.Features.Stock.Commands.Set;

internal sealed class SetStockCommandHandler
    : IRequestHandler<SetStockCommand>
{
    private readonly ILogger<SetStockCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;

    public SetStockCommandHandler(
        ILogger<SetStockCommandHandler> logger,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(
        SetStockCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Stock: {StockId}",
            nameof(SetStockCommand),
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
            nameof(SetStockCommand),
            request.StockId);
    }
}