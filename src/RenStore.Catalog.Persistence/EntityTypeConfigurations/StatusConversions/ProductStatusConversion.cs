namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

public static class ProductStatusConversion
{
    internal static string ToDatabase(ProductStatus status)
    {
        return status switch
        {
            ProductStatus.PendingModeration => "pending_moderation",
            ProductStatus.Rejected => "rejected",
            ProductStatus.Approved => "approved",
            ProductStatus.Draft => "draft",
            ProductStatus.Published => "published",
            ProductStatus.Hidden => "hidden",
            ProductStatus.Archived => "archived",
            ProductStatus.Deleted => "is_deleted",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
    
    internal static ProductStatus FromDatabase(string value)
    {
        return value switch
        {
             "pending_moderation" => ProductStatus.PendingModeration,
             "rejected" => ProductStatus.Rejected,
             "approved" => ProductStatus.Approved,
             "draft" => ProductStatus.Draft,
             "published" => ProductStatus.Published,
             "hidden" => ProductStatus.Hidden,
             "archived" => ProductStatus.Archived,
             "is_deleted" => ProductStatus.Deleted,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}