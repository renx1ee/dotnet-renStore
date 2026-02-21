using RenStore.Catalog.Domain.Aggregates.VariantDetails;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IVariantDetailRepository
{
    Task<Guid> AddAsync(
        VariantDetail detail,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantDetail> detail,
        CancellationToken cancellationToken);

    void Remove(VariantDetail detail);

    void RemoveRange(IReadOnlyCollection<VariantDetail> details);
}