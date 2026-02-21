using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.VariantDetails;

public class DetailTestBase
{
    protected VariantDetail CreateDetail()
    {
        return VariantDetail.Create(
            now: DateTimeOffset.UtcNow, 
            variantId: Guid.NewGuid(), 
            countryOfManufactureId: 15532523,
            description: "description description description description",
            composition: "composition composition composition composition",
            caringOfThings: "Caring Of Things",
            typeOfPackaging: TypeOfPacking.Box,
            modelFeatures: "model features sample",
            decorativeElements: "decorative elements",
            equipment: "equipment");
    }
}