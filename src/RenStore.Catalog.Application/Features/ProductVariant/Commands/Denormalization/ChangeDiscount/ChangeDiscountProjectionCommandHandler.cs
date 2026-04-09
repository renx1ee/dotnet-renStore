namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeDiscount;

internal sealed class ChangeDiscountProjectionCommandHandler
    : IRequestHandler<ChangeDiscountProjectionCommand>
{
    private readonly ILogger<ChangeDiscountProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    
    public ChangeDiscountProjectionCommandHandler(
        ILogger<ChangeDiscountProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection)
    {
        _logger = logger;
        _variantProjection = variantProjection;
    }

    public async Task Handle(
        ChangeDiscountProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeDiscountProjectionCommand),
            request.VariantId);

        await _variantProjection.ChangeDiscountAsync(
            now: request.OccurredAt,
            variantId: request.VariantId,
            discountPercents: request.DiscountPercents,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeDiscountProjectionCommand),
            request.VariantId);
    }
}