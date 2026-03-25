namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class ProductTestBase
{
    protected Catalog.Domain.Aggregates.Product.Product CreateProduct()
    {
        return Catalog.Domain.Aggregates.Product.Product.Create(
            sellerId: Guid.NewGuid(),
            categoryId: Guid.NewGuid(),
            subCategoryId: Guid.NewGuid(),
            now: DateTimeOffset.Now.AddHours(1));
    }
}