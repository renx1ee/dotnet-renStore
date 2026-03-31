namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

internal sealed class StockSoftDeleteCommandHandler
    : IRequestHandler<StockSoftDeleteCommand>
{
    private readonly ILogger<StockSoftDeleteCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;
    private readonly ICurrentUserService _userService;

    public StockSoftDeleteCommandHandler(
        ILogger<StockSoftDeleteCommandHandler> logger,
        IStockRepository stockRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _stockRepository = stockRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        StockSoftDeleteCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with StockId: {StockId}",
            nameof(StockSoftDeleteCommand),
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

        if (_userService.UserId is null)
            throw new ForbiddenException();
        
        stock.Delete(
            now: DateTimeOffset.UtcNow,
            updatedById: (Guid)_userService.UserId,
            updatedByRole: _userService.Role);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(StockSoftDeleteCommand),
            request.StockId);
    }
}