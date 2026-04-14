using MassTransit;

namespace RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;

internal sealed class AddToStockCommandHandler
    : IRequestHandler<AddToStockCommand>
{
    private readonly ILogger<AddToStockCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddToStockCommandHandler(
        ILogger<AddToStockCommandHandler> logger,
        IStockRepository stockRepository,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _stockRepository = stockRepository;
        _publishEndpoint = publishEndpoint;
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
        
        /*await _publishEndpoint.Publish(
            new VariantSizeCreatedIntegrationEvent(
                VariantId: request.VariantId,
                SizeId: sizeId),
            cancellationToken);*/
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(AddToStockCommand),
            request.StockId);
    }
}