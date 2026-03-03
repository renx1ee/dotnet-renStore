using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class VariantDetailProjection
    : IVariantDetailProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantDetailProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Guid> AddAsync(
        VariantDetail detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        await _context.Details.AddAsync(detail, cancellationToken);

        return detail.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantDetail> detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        var detailsList = detail as IList<VariantDetail> ?? detail.ToList();
        
        if (detailsList.Count == 0) return;

        await _context.Details.AddRangeAsync(detailsList, cancellationToken);
    }

    public void Remove(VariantDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);

        _context.Details.Remove(detail);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantDetail> details)
    {
        ArgumentNullException.ThrowIfNull(details);

        _context.Details.RemoveRange(details);
    }
}