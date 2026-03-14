using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Domain.DomainService;

public interface IImageSortOrderService
{
    void Reorder(
        IReadOnlyCollection<VariantImage> images,
        IReadOnlyList<Guid> orderedImageIds,
        DateTimeOffset now);
}