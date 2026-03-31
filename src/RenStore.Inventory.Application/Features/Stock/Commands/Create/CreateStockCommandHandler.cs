namespace RenStore.Inventory.Application.Features.Stock.Commands.Create;

internal sealed class CreateStockCommandHandler
    : IRequestHandler<CreateStockCommand, Guid>
{
    private readonly ILogger<CreateStockCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;
    private readonly IStockProjection _stockProjection;

    public CreateStockCommandHandler(
        ILogger<CreateStockCommandHandler> logger,
        IStockRepository stockRepository,
        IStockProjection stockProjection)
    {
        _logger = logger;
        _stockRepository = stockRepository;
        _stockProjection = stockProjection;
    }
    
    public async Task<Guid> Handle(
        CreateStockCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(CreateStockCommand),
            request.VariantId,
            request.SizeId);

        var stockExists = await _stockProjection
            .IsExistsAsync(
                sizeId: request.SizeId,
                variantId: request.VariantId,
                cancellationToken: cancellationToken);

        if (stockExists)
        {
            throw new DomainException(
                "Variant Stock already exists.");
        }

        var stock = VariantStock.Create(
            now: DateTimeOffset.UtcNow, 
            sizeId: request.SizeId,
            variantId: request.VariantId,
            initialStock: request.InitialStock);

        await _stockRepository.SaveAsync(
            stock: stock,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}, StockId: {StockId}",
            nameof(CreateStockCommand),
            request.VariantId,
            request.SizeId,
            stock.Id);

        return stock.Id;
    }
}