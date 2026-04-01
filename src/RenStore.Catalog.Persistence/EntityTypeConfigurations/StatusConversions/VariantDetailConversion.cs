using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

internal static class VariantDetailConversion
{
    internal static string TypeOfPackingToDatabase(TypeOfPacking? typeOfPacking)
    {
        return typeOfPacking switch
        {
            TypeOfPacking.Box => "box",
            TypeOfPacking.Package => "package",
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfPacking))
        };
    }

    internal static TypeOfPacking TypeOfPackingFromDatabase(string typeOfPacking)
    {
        return typeOfPacking switch
        {
            "box" => TypeOfPacking.Box,
            "package" => TypeOfPacking.Package,
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfPacking))
        };
    }
}