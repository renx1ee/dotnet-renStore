using RenStore.Inventory.Application.Abstractions.Queries;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Inventory.Application.Features.Stock.Queries.FindByVariantId;

internal sealed class FindStockByVariantIdQueryHandler
    : IRequestHandler<FindStockByVariantIdQuery, IReadOnlyList<VariantStockDto?>>
{
    private readonly ILogger<FindStockByVariantIdQueryHandler> _logger;
    private readonly IStockQuery _stockQuery;
    private readonly ICurrentUserService _currentUserService;

    public FindStockByVariantIdQueryHandler(
        ILogger<FindStockByVariantIdQueryHandler> logger,
        IStockQuery stockQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _stockQuery = stockQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<IReadOnlyList<VariantStockDto?>> Handle(
        FindStockByVariantIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}",
            nameof(FindStockByVariantIdQuery),
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
            isDeleted: isDeletedFilter,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} Handled. VariantId: {VariantId}",
            nameof(FindStockByVariantIdQuery),
            request.VariantId);
        
        return result;
    }
}