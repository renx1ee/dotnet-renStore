using RenStore.Inventory.Application.Abstractions.Queries;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Inventory.Application.Features.Stock.Queries.FindById;

internal sealed class FindStockByIdAsyncQueryHandler
    : IRequestHandler<FindStockByIdAsyncQuery, VariantStockDto?>
{
    private readonly ILogger<FindStockByIdAsyncQueryHandler> _logger;
    private readonly IStockQuery _stockQuery;
    private readonly ICurrentUserService _currentUserService;

    public FindStockByIdAsyncQueryHandler(
        ILogger<FindStockByIdAsyncQueryHandler> logger,
        IStockQuery stockQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _stockQuery = stockQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<VariantStockDto?> Handle(
        FindStockByIdAsyncQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with StockId: {StockId}",
            nameof(FindStockByIdAsyncQuery),
            request.StockId);
        
        // TODO:
        var canViewDeleted = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support => true,
            _ => false
        };

        var isDeletedFilter = canViewDeleted 
            ? request.IsDeleted 
            : false;

        var result = await _stockQuery.FindByIdAsync(
            id: request.StockId,
            isDeleted: isDeletedFilter,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} Handled. StockId: {StockId}",
            nameof(FindStockByIdAsyncQuery),
            request.StockId);
        
        return result;
    }
}