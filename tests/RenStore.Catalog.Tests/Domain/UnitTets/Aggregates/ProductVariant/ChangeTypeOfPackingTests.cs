using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class ChangeTypeOfPackingTests : ProductVariantTestBase
{
    [Theory]
    [InlineData(TypeOfPacking.Package)]
    public void Should_Raise_TypeOfPackingUpdated_Event(
        TypeOfPacking newTypeOfPacking)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New Equipment";

        var variant = CreateValidProductVariant();
        
        variant.AddDetails(
            now: now,
            countryOfManufactureId: 342,
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPackaging: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeDetailsTypeOfPacking(
            now: now,
            typeOfPacking: newTypeOfPacking);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsTypeOfPackingUpdated>(@event);

        // Assert: event
        Assert.Equal(newTypeOfPacking, result.TypeOfPacking);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(newTypeOfPacking, variant.Details.TypeOfPacking);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_TypeOfPackingTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        
        variant.AddDetails(
            now: now,
            countryOfManufactureId: 342,
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPackaging: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act & Assert
        variant.ChangeDetailsTypeOfPacking(
            now: now,
            typeOfPacking: variant.Details!.TypeOfPacking!.Value);

        Assert.Empty(variant.GetUncommittedEvents());
    }
}