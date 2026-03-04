using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class VariantAttributeProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantAttributeProjection
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public VariantAttributeProjection(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(
        VariantAttributeReadModel attribute,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        await _context.Attributes.AddAsync(attribute, cancellationToken);

        return attribute.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantAttributeReadModel> attributes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        var attributesList = attributes as IList<VariantAttributeReadModel> ?? attributes.ToList();
        
        if(attributesList.Count == 0) return;

        await _context.Attributes.AddRangeAsync(attributesList, cancellationToken);
    }
    
    public void Remove(VariantAttributeReadModel attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        _context.Attributes.Remove(attribute);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantAttributeReadModel> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        _context.Attributes.RemoveRange(attributes);
    }
}