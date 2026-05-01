using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Application.Abstractions.Services;

namespace RenStore.Catalog.Persistence.Services;

internal sealed class ArticleService 
    : IArticleService
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