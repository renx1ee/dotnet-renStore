using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.Converters;

internal sealed class TypeOfPackingConverter : ValueConverter<TypeOfPacking?, string?>
{
    public TypeOfPackingConverter()
        : base(
            d => d == null 
                ? null
                : VariantDetailConversion.TypeOfPackingToDatabase(d),
            d => d == null
                ? null
                : VariantDetailConversion.TypeOfPackingFromDatabase(d))
    {
    }
}