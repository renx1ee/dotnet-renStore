using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsCountryOfManufactureIdTests : ProductVariantTestBase
{
    [Theory]
    [InlineData(12)]
    [InlineData(52)]
    public void Should_Raise_CountryOfManufactureIdUpdated_Event(
        int countryId)
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

        // Act
        variant.ChangeDetailsCountryOfManufactureId(
            now: now,
            countryOfManufactureId: countryId);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCountryOfManufactureIdUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(countryId, result.CountryOfManufactureId);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(countryId, variant.Details.CountryOfManufactureId);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_CountryOfManufactureIdTheSame()
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
        variant.ChangeDetailsCountryOfManufactureId(
            now: now,
            countryOfManufactureId: variant.Details.CountryOfManufactureId);

        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Should_Throw_NoRise_Where_IncorrectCountryId(
        int countryId)
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
        Assert.Throws<DomainException>(() =>
            variant.ChangeDetailsCountryOfManufactureId(
                now: now,
                countryOfManufactureId: countryId));
    }
}