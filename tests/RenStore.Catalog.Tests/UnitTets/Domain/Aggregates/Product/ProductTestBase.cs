namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Product;

public class ProductTestBase
{
    protected Catalog.Domain.Aggregates.Product.Product CreateProduct()
    {
        return Catalog.Domain.Aggregates.Product.Product.Create(
            sellerId: 12345,
            subCategoryId: 3242,
            now: DateTimeOffset.Now.AddHours(1));
    }
}