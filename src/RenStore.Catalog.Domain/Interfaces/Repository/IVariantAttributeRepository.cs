using RenStore.Catalog.Domain.Aggregates.Attribute;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IVariantAttributeRepository
{
    Task<VariantAttribute?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantAttribute attribute,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantAttribute> attributes,
        CancellationToken cancellationToken);

    void Remove(VariantAttribute attribute);

    void RemoveRange(IReadOnlyCollection<VariantAttribute> attributes);
}