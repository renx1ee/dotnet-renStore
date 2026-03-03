namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class ProductTestBase
{
    protected Catalog.Domain.Aggregates.Product.Product CreateProduct()
    {
        return Catalog.Domain.Aggregates.Product.Product.Create(
            sellerId: 12345,
            subCategoryId: Guid.NewGuid(),
            now: DateTimeOffset.Now.AddHours(1));
    }
}