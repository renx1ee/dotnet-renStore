using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByVariantId;

internal sealed class FindFullProductPageByVariantIdQueryHandler
    : IRequestHandler<FindFullProductPageByVariantIdQuery, FullProductPageDto?>
{
    private readonly ILogger<FindFullProductPageByVariantIdQueryHandler> _logger;
    private readonly IFullProductQuery _fullProductQuery;

    public FindFullProductPageByVariantIdQueryHandler(
        ILogger<FindFullProductPageByVariantIdQueryHandler> logger,
        IFullProductQuery fullProductQuery)
    {
        _logger = logger;
        _fullProductQuery = fullProductQuery;
    }
    
    public async Task<FullProductPageDto?> Handle(
        FindFullProductPageByVariantIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(FindFullProductPageByVariantIdQuery),
            request.VariantId);

        var fullPage = await _fullProductQuery.FindFullAsync(
            variantId: request.VariantId,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(FindFullProductPageByVariantIdQuery),
            request.VariantId);

        return fullPage;
    }
}