using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantAttributeProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantAttributeReadModel attribute,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantAttributeReadModel> attributes,
        CancellationToken cancellationToken);

    void Remove(VariantAttributeReadModel attribute);

    void RemoveRange(IReadOnlyCollection<VariantAttributeReadModel> attributes);
}