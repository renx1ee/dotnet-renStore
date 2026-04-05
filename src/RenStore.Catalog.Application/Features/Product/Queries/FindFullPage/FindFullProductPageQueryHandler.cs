using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPage;

internal sealed class FindFullProductPageQueryHandler
    : IRequestHandler<FindFullProductPageQuery, FullProductPageDto?>
{
    private readonly ILogger<FindFullProductPageQueryHandler> _logger;
    private readonly IFullProductQuery _fullProductQuery;

    public FindFullProductPageQueryHandler(
        ILogger<FindFullProductPageQueryHandler> logger,
        IFullProductQuery fullProductQuery)
    {
        _logger = logger;
        _fullProductQuery = fullProductQuery;
    }
    
    public async Task<FullProductPageDto?> Handle(
        FindFullProductPageQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(FindFullProductPageQuery),
            request.VariantId);

        var fullPage = await _fullProductQuery.FindFullAsync(
            variantId: request.VariantId,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(FindFullProductPageQuery),
            request.VariantId);

        return fullPage;
    }
}