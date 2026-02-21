using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;

namespace RenStore.Catalog.Domain.DomainService;

public interface IVariantPublishService
{
    void Publish(
        DateTimeOffset now,
        Product product, 
        IReadOnlyCollection<ProductVariant> variants,
        IReadOnlyCollection<Attribute> attributes,
        IReadOnlyCollection<VariantImage> images,
        VariantDetail detail);
}