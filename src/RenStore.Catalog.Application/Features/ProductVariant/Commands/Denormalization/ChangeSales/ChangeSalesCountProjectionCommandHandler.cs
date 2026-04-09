namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSales;

internal sealed class ChangeSalesCountProjectionCommandHandler
    : IRequestHandler<ChangeSalesCountProjectionCommand>
{
    private readonly ILogger<ChangeSalesCountProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    
    public ChangeSalesCountProjectionCommandHandler(
        ILogger<ChangeSalesCountProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection)
    {
        _logger = logger;
        _variantProjection = variantProjection;
    }

    public async Task Handle(
        ChangeSalesCountProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeSalesCountProjectionCommand),
            request.VariantId);

        await _variantProjection.ChangeSalesAsync(
            now: request.OccurredAt,
            variantId: request.VariantId,
            sales: request.Sales,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeSalesCountProjectionCommand),
            request.VariantId);
    }
}