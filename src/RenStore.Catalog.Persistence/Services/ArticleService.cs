using Microsoft.EntityFrameworkCore;

namespace RenStore.Catalog.Persistence.Services;

internal sealed class ArticleService 
    : RenStore.Catalog.Application.Service.IArticleService
{
    private readonly CatalogDbContext _context;

    public ArticleService(CatalogDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> GenerateAsync(CancellationToken cancellationToken)
    {
        return await _context.Database
            .SqlQueryRaw<long>("SELECT nextval('product_article_seq') as \"Value\"")
            .SingleAsync(cancellationToken);
    }
}