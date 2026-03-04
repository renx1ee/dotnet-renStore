using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class VariantDetailProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantDetailProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantDetailProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        VariantDetailReadModel detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        await _context.Details.AddAsync(detail, cancellationToken);

        return detail.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantDetailReadModel> detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        var detailsList = detail as IList<VariantDetailReadModel> ?? detail.ToList();
        
        if (detailsList.Count == 0) return;

        await _context.Details.AddRangeAsync(detailsList, cancellationToken);
    }

    public void Remove(VariantDetailReadModel detail)
    {
        ArgumentNullException.ThrowIfNull(detail);

        _context.Details.Remove(detail);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantDetailReadModel> details)
    {
        ArgumentNullException.ThrowIfNull(details);

        _context.Details.RemoveRange(details);
    }
}