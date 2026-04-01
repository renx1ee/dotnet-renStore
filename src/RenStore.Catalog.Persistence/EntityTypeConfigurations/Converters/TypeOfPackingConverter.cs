using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

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