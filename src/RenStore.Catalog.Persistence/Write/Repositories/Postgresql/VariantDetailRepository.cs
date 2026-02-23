using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class VariantDetailRepository
    : IVariantDetailRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public VariantDetailRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<VariantDetail?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (!events.Any()) return null;
        
        return VariantDetail.Rehydrate(events);
    }

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