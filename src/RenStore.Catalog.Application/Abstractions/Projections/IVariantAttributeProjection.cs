using RenStore.Catalog.Domain.Aggregates.Attribute;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantAttributeProjection
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