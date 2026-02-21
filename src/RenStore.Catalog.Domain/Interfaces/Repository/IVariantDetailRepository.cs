using RenStore.Catalog.Domain.Aggregates.VariantDetails;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IVariantDetailRepository
{
    Task<VariantDetail?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantDetail detail,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantDetail> detail,
        CancellationToken cancellationToken);

    void Remove(VariantDetail detail);

    void RemoveRange(IReadOnlyCollection<VariantDetail> details);
}