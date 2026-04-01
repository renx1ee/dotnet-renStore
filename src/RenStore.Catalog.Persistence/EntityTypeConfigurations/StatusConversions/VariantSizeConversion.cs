using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

internal static class VariantSizeConversion
{
    internal static string SizeTypeToDatabase(SizeType sizeType)
    {
        return sizeType switch
        {
            SizeType.Clothes => "clothes",
            SizeType.Shoes => "shoes"
        };
    }
}