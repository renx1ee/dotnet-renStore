using RenStore.Catalog.Application.Filters;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.SearchVariants;

internal sealed class SearchVariantsQueryHandler
    : IRequestHandler<SearchVariantsQuery, IReadOnlyList<CatalogReadModel>>
{
    private readonly ILogger<SearchVariantsQueryHandler> _logger;
    private readonly ICatalogQuery query;
    
    public SearchVariantsQueryHandler(
        ILogger<SearchVariantsQueryHandler> logger,
        ICatalogQuery query)
    {
        _logger = logger;
        this.query = query;
    }
    
    public async Task<IReadOnlyList<CatalogReadModel>> Handle(
        SearchVariantsQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Query}.", nameof(SearchVariantsQuery));

        var result = await query.SearchAsync(
            filter: new CatalogSearchFilter()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Descending = request.Descending,
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                ColorId = request.ColorId,
                SortBy = request.SortBy,
                Search = request.Search
            },
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("{Query} handled.", nameof(SearchVariantsQuery));

        return result;
    }
}