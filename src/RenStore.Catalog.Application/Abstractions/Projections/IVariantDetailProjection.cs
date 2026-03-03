using RenStore.Catalog.Domain.Aggregates.VariantDetails;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantDetailProjection
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