namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindActiveSizePrice;

internal sealed class FindActiveVariantSizePriceBySizeIdQueryHandler
    (ILogger<FindActiveVariantSizePriceBySizeIdQueryHandler> logger,
     IProductVariantQuery productVariantQuery,
     IPriceHistoryQuery priceHistoryQuery)
    : IRequestHandler<FindActiveVariantSizePriceBySizeIdQuery, PriceHistoryReadModel?>
{
    public async Task<PriceHistoryReadModel?> Handle(
        FindActiveVariantSizePriceBySizeIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindActiveVariantSizePriceBySizeIdQuery),
            request.VariantId,
            request.SizeId);

        var variant = await productVariantQuery.FindByIdAsync(
            id: request.VariantId,
            cancellationToken: cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(ProductVariantReadModel), request.VariantId);
        }

        if (variant.Status != ProductVariantStatus.Published)
        {
            throw new ForbiddenException();
        }

        var currentPrice = await priceHistoryQuery
            .FindActiveBySizeIdAsync(
                sizeId: request.SizeId,
                cancellationToken: cancellationToken);
        
        logger.LogInformation(
            "{Query} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindActiveVariantSizePriceBySizeIdQuery),
            request.VariantId,
            request.SizeId);
        
        return currentPrice;
    }
}