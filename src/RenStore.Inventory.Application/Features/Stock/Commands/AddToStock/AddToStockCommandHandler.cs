using MassTransit;

namespace RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;

internal sealed class AddToStockCommandHandler
    : IRequestHandler<AddToStockCommand>
{
    private readonly ILogger<AddToStockCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;

    public AddToStockCommandHandler(
        ILogger<AddToStockCommandHandler> logger,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(
        AddToStockCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with StockId: {StockId}",
            nameof(AddToStockCommand),
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
        
        stock.AddToStock(
            now: DateTimeOffset.UtcNow,
            count: request.Count);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(AddToStockCommand),
            request.StockId);
    }
}