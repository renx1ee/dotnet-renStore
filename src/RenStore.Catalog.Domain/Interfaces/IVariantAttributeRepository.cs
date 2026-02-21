using RenStore.Catalog.Domain.Aggregates.Attribute;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IVariantAttributeRepository
{
    Task<Guid> AddAsync(
        VariantAttribute attribute,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantAttribute> attributes,
        CancellationToken cancellationToken);

    void Remove(VariantAttribute attribute);

    void RemoveRange(IReadOnlyCollection<VariantAttribute> attributes);
}