namespace RenStore.Inventory.Application.Features.Stock.Commands.Restore;

internal sealed class RestoreStockCommandHandler
    : IRequestHandler<RestoreStockCommand>
{
    private readonly ILogger<RestoreStockCommandHandler> _logger;
    private readonly IStockRepository _stockRepository;
    private readonly ICurrentUserService _userService;

    public RestoreStockCommandHandler(
        ILogger<RestoreStockCommandHandler> logger,
        IStockRepository stockRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _stockRepository = stockRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RestoreStockCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with StockId: {StockId}",
            nameof(RestoreStockCommand),
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
        
        stock.Restore(
            now: DateTimeOffset.UtcNow,
            updatedById: (Guid)_userService.UserId,
            updatedByRole: _userService.Role);

        await _stockRepository.SaveAsync(stock, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. StockId: {StockId}",
            nameof(RestoreStockCommand),
            request.StockId);
    }
}