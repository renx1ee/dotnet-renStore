using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsCountryOfManufactureIdTests : ProductVariantTestBase
{
    [Theory]
    [InlineData("Country")]
    public void Should_Raise_CountryOfManufactureIdUpdated_Event(
        string country)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        
        variant.AddDetails(
            now: now,
            countryOfManufacture: "Samplecountry",
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeDetailsCountryOfManufactureId(
            now: now,
            countryOfManufacture: country);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCountryOfManufactureIdUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(country, result.CountryOfManufacture);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(country, variant.Details.CountryOfManufacture.Name);
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
            countryOfManufacture: "Samplecountry",
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act & Assert
        variant.ChangeDetailsCountryOfManufactureId(
            now: now,
            countryOfManufacture: variant.Details.CountryOfManufacture.Name);

        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1")]
    public void Should_Throw_NoRise_Where_IncorrectCountryId(
        string country)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        
        variant.AddDetails(
            now: now,
            countryOfManufacture: "Samplecountry",
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.ChangeDetailsCountryOfManufactureId(
                now: now,
                countryOfManufacture: country));
    }
}