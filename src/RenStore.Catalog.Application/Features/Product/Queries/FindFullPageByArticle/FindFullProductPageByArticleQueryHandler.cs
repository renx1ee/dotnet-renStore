using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindFullPageByArticle;

internal sealed class FindFullProductPageByArticleQueryHandler
    : IRequestHandler<FindFullProductPageByArticleQuery, FullProductPageDto?>
{
    private readonly ILogger<FindFullProductPageByArticleQueryHandler> _logger;
    private readonly IFullProductQuery _fullProductQuery;

    public FindFullProductPageByArticleQueryHandler(
        ILogger<FindFullProductPageByArticleQueryHandler> logger,
        IFullProductQuery fullProductQuery)
    {
        _logger = logger;
        _fullProductQuery = fullProductQuery;
    }
    
    public async Task<FullProductPageDto?> Handle(
        FindFullProductPageByArticleQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with Article: {Article}",
            nameof(FindFullProductPageByArticleQuery),
            request.Article);

        var fullPage = await _fullProductQuery.FindFullAsync(
            article: request.Article,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. Article: {Article}",
            nameof(FindFullProductPageByArticleQuery),
            request.Article);

        return fullPage;
    }
}