using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class VariantAttributeRepository
    : IVariantAttributeRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public VariantAttributeRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<VariantAttribute?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (!events.Any()) return null;
        
        return VariantAttribute.Rehydrate(events);
    }

    public async Task<Guid> AddAsync(
        VariantAttribute attribute,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        await _context.Attributes.AddAsync(attribute, cancellationToken);

        return attribute.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantAttribute> attributes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        var attributesList = attributes as IList<VariantAttribute> ?? attributes.ToList();
        
        if(attributesList.Count == 0) return;

        await _context.Attributes.AddRangeAsync(attributesList, cancellationToken);
    }
    
    public void Remove(VariantAttribute attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        _context.Attributes.Remove(attribute);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantAttribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        _context.Attributes.RemoveRange(attributes);
    }
}