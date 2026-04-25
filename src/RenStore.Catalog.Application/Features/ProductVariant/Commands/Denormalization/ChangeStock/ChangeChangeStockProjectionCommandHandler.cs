using RenStore.Catalog.Application.Abstractions.Repository;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeStock;

internal sealed class ChangeChangeStockProjectionCommandHandler
    : IRequestHandler<ChangeChangeStockProjectionCommand>
{
    private readonly ILogger<ChangeChangeStockProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    private readonly IProductVariantSizeProjection _sizeProjection;
    private readonly IProductVariantSizeRepository _sizeReadRepository;
    
    public ChangeChangeStockProjectionCommandHandler(
        ILogger<ChangeChangeStockProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection,
        IProductVariantSizeProjection sizeProjection,
        IProductVariantSizeRepository sizeReadRepository)
    {
        _logger             = logger;
        _variantProjection  = variantProjection;
        _sizeProjection     = sizeProjection;
        _sizeReadRepository = sizeReadRepository;
    }

    public async Task Handle(
        ChangeChangeStockProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeChangeStockProjectionCommand),
            request.VariantId);
        
        await _sizeProjection.ChangeStockAsync(
            sizeId:    request.SizeId,
            variantId: request.VariantId,
            now:       request.OccurredAt,
            stock:     request.InStock,
            sales:     request.Sales,
            cancellationToken: cancellationToken);

        var sizes = await _sizeReadRepository.GetByVariantIdAsync(
            variantId: request.VariantId,
            cancellationToken: cancellationToken);

        if (sizes.Count == 0)
            throw new InvalidOperationException(nameof(request.VariantId));

        var totalInStock = sizes.Sum(x => 
            x.Id == request.SizeId 
            ? request.InStock 
            : x.InStock); 
        
        var totalSales = sizes.Sum(x => 
            x.Id == request.SizeId 
            ? request.Sales 
            : x.SalesCount); 

        await _variantProjection.ChangeStockAsync(
            now:       request.OccurredAt,
            variantId: request.VariantId, 
            stock:     totalInStock ?? 0,
            sales:     totalSales   ?? 0,
            cancellationToken: cancellationToken);
        
        await _sizeProjection.CommitAsync(cancellationToken);
        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeChangeStockProjectionCommand),
            request.VariantId);
    }
}