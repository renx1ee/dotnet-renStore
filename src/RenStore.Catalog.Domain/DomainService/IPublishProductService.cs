using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Domain.DomainService;

public interface IPublishProductService
{
    void Publish(
        DateTimeOffset now,
        Product product,
        IReadOnlyCollection<ProductVariant> variants,
        IReadOnlyDictionary<Guid, IReadOnlyCollection<VariantImage>> imagesByVariants);
}