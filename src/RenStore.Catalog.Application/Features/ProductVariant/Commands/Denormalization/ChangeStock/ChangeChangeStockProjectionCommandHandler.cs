namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeStock;

internal sealed class ChangeChangeStockProjectionCommandHandler
    : IRequestHandler<ChangeChangeStockProjectionCommand>
{
    private readonly ILogger<ChangeChangeStockProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    
    public ChangeChangeStockProjectionCommandHandler(
        ILogger<ChangeChangeStockProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection)
    {
        _logger = logger;
        _variantProjection = variantProjection;
    }

    public async Task Handle(
        ChangeChangeStockProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeChangeStockProjectionCommand),
            request.VariantId);

        await _variantProjection.ChangeStockAsync(
            now: request.OccurredAt,
            variantId: request.VariantId,
            stock: request.InStock,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeChangeStockProjectionCommand),
            request.VariantId);
    }
}