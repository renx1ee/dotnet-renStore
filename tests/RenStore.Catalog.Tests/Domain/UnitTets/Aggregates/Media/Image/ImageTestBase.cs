using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class ImageTestBase
{
    protected VariantImage CreateValidImage(
        bool isMain = false)
    {
        return VariantImage.Create(
            now: DateTimeOffset.UtcNow, 
            variantId: Guid.NewGuid(),
            originalFileName: "photo.jpg",
            storagePath: "/storage/products/2026/02/17/photo.jpg",
            fileSizeBytes: 1024,
            isMain: isMain,
            sortOrder: 1,
            weight: 800,
            height: 600);
    }
}