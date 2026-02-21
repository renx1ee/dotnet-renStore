using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IVariantSizeRepository
{
    Task<Guid> CreateAsync(
        VariantSize size,
        CancellationToken cancellationToken);

    Task CreateRangeAsync(
        IReadOnlyCollection<VariantSize> sizes,
        CancellationToken cancellationToken);

    void Remove(VariantSize size);

    void RemoveRange(IReadOnlyCollection<VariantSize> sizes);
}