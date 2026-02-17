using RenStore.Catalog.Domain.Aggregates.Attribute;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Attribute;

public class AttributeTestBase
{
    protected VariantAttribute CreateAttribute()
    {
        return VariantAttribute.Create(
            now: DateTimeOffset.Now,
            variantId: Guid.NewGuid(),
            key: "qwertyuiopasdfg",
            value: "qwertyuiopasdfg");
    }
}