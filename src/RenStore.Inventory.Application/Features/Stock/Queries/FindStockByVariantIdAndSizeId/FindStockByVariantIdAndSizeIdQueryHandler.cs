using RenStore.Inventory.Application.Abstractions.Queries;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Inventory.Application.Features.Stock.Queries.FindStockByVariantIdAndSizeId;

internal sealed class FindStockByVariantIdAndSizeIdQueryHandler
    : IRequestHandler<FindStockByVariantIdAndSizeIdQuery, VariantStockDto?>
{
    private readonly ILogger<FindStockByVariantIdAndSizeIdQueryHandler> _logger;
    private readonly IStockQuery _stockQuery;
    private readonly ICurrentUserService _currentUserService;

    public FindStockByVariantIdAndSizeIdQueryHandler(
        ILogger<FindStockByVariantIdAndSizeIdQueryHandler> logger,
        IStockQuery stockQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _stockQuery = stockQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<VariantStockDto?> Handle(
        FindStockByVariantIdAndSizeIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId},",
            nameof(FindStockByVariantIdAndSizeIdQuery),
            request.VariantId);
        
        var canViewDeleted = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support => true,
            _ => false
        };

        var isDeletedFilter = canViewDeleted 
            ? request.IsDeleted 
            : false;

        var result = await _stockQuery.FindByVariantIdAsync(
            variantId: request.VariantId,
            sizeId: request.SizeId,
            isDeleted: isDeletedFilter,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} Handled. VariantId: {VariantId}",
            nameof(FindStockByVariantIdAndSizeIdQuery),
            request.VariantId);
        
        return result;
    }
}