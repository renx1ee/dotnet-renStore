using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class ProductVariantTestBase
{
    protected Catalog.Domain.Aggregates.Variant.ProductVariant CreateValidProductVariant()
    {
        return RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
            .Create(
                now: DateTimeOffset.UtcNow,
                productId: Guid.NewGuid(),
                colorId: 1231,
                name: "Sample product variant name",
                sizeSystem: SizeSystem.EU,
                sizeType: SizeType.Clothes,
                article: 42243,
                url: "https://renstore/catallg/todo/fwfwfew");
    }
}