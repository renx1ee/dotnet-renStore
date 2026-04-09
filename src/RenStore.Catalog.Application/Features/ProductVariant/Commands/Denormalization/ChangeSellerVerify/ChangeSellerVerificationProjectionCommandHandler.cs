namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSellerVerify;

internal sealed class ChangeSellerVerificationProjectionCommandHandler
    : IRequestHandler<ChangeSellerVerificationProjectionCommand>
{
    private readonly ILogger<ChangeSellerVerificationProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    
    public ChangeSellerVerificationProjectionCommandHandler(
        ILogger<ChangeSellerVerificationProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection)
    {
        _logger = logger;
        _variantProjection = variantProjection;
    }

    public async Task Handle(
        ChangeSellerVerificationProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeSellerVerificationProjectionCommand),
            request.VariantId);

        await _variantProjection.ChangeSellerVerificationAsync(
            now: request.OccurredAt,
            variantId: request.VariantId,
            isVerified: request.IsVerified,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeSellerVerificationProjectionCommand),
            request.VariantId);
    }
}