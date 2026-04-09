namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeReviewsCount;

internal sealed class ChangeReviewsCountProjectionCommandHandler
    : IRequestHandler<ChangeReviewsCountProjectionCommand>
{
    private readonly ILogger<ChangeReviewsCountProjectionCommandHandler> _logger;
    private readonly IProductVariantProjection _variantProjection;
    
    public ChangeReviewsCountProjectionCommandHandler(
        ILogger<ChangeReviewsCountProjectionCommandHandler> logger,
        IProductVariantProjection variantProjection)
    {
        _logger = logger;
        _variantProjection = variantProjection;
    }

    public async Task Handle(
        ChangeReviewsCountProjectionCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeReviewsCountProjectionCommand),
            request.VariantId);

        await _variantProjection.ChangeReviewsCountAsync(
            now: request.OccurredAt,
            variantId: request.VariantId,
            reviewsCount: request.Count,
            averageRating: request.AverageRating,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeReviewsCountProjectionCommand),
            request.VariantId);
    }
}