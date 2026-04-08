using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByUrlSlug;

internal sealed class FindFullProductPageByUrlSlugQueryHandler
    : IRequestHandler<FindFullProductPageByUrlSlugQuery, FullProductPageDto>
{
    private readonly ILogger<FindFullProductPageByUrlSlugQueryHandler> _logger;
    private readonly IFullProductQuery _fullProductQuery;

    public FindFullProductPageByUrlSlugQueryHandler(
        ILogger<FindFullProductPageByUrlSlugQueryHandler> logger,
        IFullProductQuery fullProductQuery)
    {
        _logger = logger;
        _fullProductQuery = fullProductQuery;
    }
    
    public async Task<FullProductPageDto?> Handle(
        FindFullProductPageByUrlSlugQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with Article: {Article}",
            nameof(FindFullProductPageByUrlSlugQuery),
            request.UrlSlug);

        var fullPage = await _fullProductQuery.FindFullAsync(
            urlSlug: request.UrlSlug,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. Article: {Article}",
            nameof(FindFullProductPageByUrlSlugQuery),
            request.UrlSlug);

        return fullPage;
    }
}