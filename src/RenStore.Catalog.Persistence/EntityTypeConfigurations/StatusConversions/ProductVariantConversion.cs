namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

internal static class ProductVariantConversion
{
    internal static string StatusToDatabase(ProductVariantStatus status)
    {
        return status switch
        {
            ProductVariantStatus.Published => "published",
            ProductVariantStatus.Deleted => "is_deleted",
            ProductVariantStatus.Draft => "draft",
            ProductVariantStatus.Archived => "archived",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
    
    internal static ProductVariantStatus StatusFromDatabase(string value)
    {
        return value switch
        {
            "published" => ProductVariantStatus.Published,
            "is_deleted" => ProductVariantStatus.Deleted,
            "draft" => ProductVariantStatus.Draft,
            "archived" => ProductVariantStatus.Archived,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
    
    internal static string SizeSystemToDatabase(SizeSystem system)
    {
        return system switch
        {
            SizeSystem.RU => "ru",
            SizeSystem.US => "us",
            SizeSystem.EU => "eu",
            _ => throw new ArgumentOutOfRangeException(nameof(system))
        };
    }
    
    internal static SizeSystem SizeSystemFromDatabase(string value)
    {
        return value switch
        {
            "ru" => SizeSystem.RU,
            "us" => SizeSystem.US,
            "eu" => SizeSystem.EU,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
    
    internal static string SizeTypeToDatabase(SizeType type)
    {
        return type switch
        {
            SizeType.Shoes => "shoes",
            SizeType.Clothes => "clothes",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
    
    internal static SizeType SizeTypeFromDatabase(string value)
    {
        return value switch
        {
            "shoes" => SizeType.Shoes,
            "clothes" => SizeType.Clothes,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}