using RenStore.Inventory.Application.Abstractions.ReadRepository;

namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

internal sealed class StockSoftDeleteCommandHandler
    : IRequestHandler<StockSoftDeleteCommand>
{
    private readonly ILogger<StockSoftDeleteCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;
    private readonly IStockReadRepository _stockReadRepository;
    private readonly ICurrentUserService _userService;

    public StockSoftDeleteCommandHandler(
        ILogger<StockSoftDeleteCommandHandler> logger,
        IStockRepository stockRepository,
        IStockReadRepository stockReadRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _stockRepository = stockRepository;
        _stockReadRepository = stockReadRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        StockSoftDeleteCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(StockSoftDeleteCommand),
            request.VariantId,
            request.SizeId);

        var existingStock = await _stockReadRepository.GetAsync(
            variantId: request.VariantId,
            sizeId: request.SizeId,
            cancellationToken: cancellationToken);
        
        if (existingStock is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStock),
                request.VariantId);
        }
        
        var stock = await _stockRepository.GetAsync(
            stockId: existingStock.Id,
            cancellationToken: cancellationToken);

        if (stock is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStock),
                existingStock.Id);
        }

        if (_userService.UserId is null)
            throw new ForbiddenException();
        
        stock.Delete(
            now: DateTimeOffset.UtcNow,
            updatedById: (Guid)_userService.UserId,
            updatedByRole: _userService.Role);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(StockSoftDeleteCommand),
            request.VariantId,
            request.SizeId);
    }
}