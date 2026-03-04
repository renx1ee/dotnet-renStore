using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantDetailProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantDetailReadModel detail,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantDetailReadModel> detail,
        CancellationToken cancellationToken);

    void Remove(VariantDetailReadModel detail);

    void RemoveRange(IReadOnlyCollection<VariantDetailReadModel> details);
}