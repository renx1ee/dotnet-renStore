using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsDecorativeElementsTests : ProductVariantTestBase
{
    private const string MaxDecorativeElementsLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New decorative elements")]
    [InlineData(" New decorative elements ")]
    public void Should_Raise_DecorativeElementsUpdated_Event(
        string newDecorativeElements)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New decorative elements";
        
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
        variant.ChangeDetailsDecorativeElements(
            now: now,
            decorativeElements: newDecorativeElements);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsDecorativeElementsUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.DecorativeElements);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, variant.Details.DecorativeElements);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_DecorativeElementsTheSame()
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
        variant.ChangeDetailsDecorativeElements(
            now: now,
            decorativeElements: variant.Details.DecorativeElements);
        
        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxDecorativeElementsLength)]
    public void Should_Throw_Where_DecorativeElementsIsIncorrect(
        string decorativeElements)
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
            variant.ChangeDetailsDecorativeElements(
                now: now,
                decorativeElements: decorativeElements));
    }
}