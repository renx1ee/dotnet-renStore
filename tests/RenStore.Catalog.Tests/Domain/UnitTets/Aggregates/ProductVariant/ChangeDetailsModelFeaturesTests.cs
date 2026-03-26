using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsModelFeaturesTests : ProductVariantTestBase
{
    private const string MaxModelFeaturesLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New sample sample model features")]
    [InlineData(" New sample sample model features ")]
    public void Should_Raise_ModelFeaturesUpdated_Event(
        string newModelFeatures)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New sample sample model features";
        
        var variant = CreateValidProductVariant();
        
        variant.AddDetails(
            now: now,
            countryOfManufactureId: 342,
            description: "Tests descriptiondescriptiondescrip",
            composition: "Composition fwwfwfwf",
            caringOfThings: "caring of things things v things things",
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeDetailsModelFeatures(
            now: now,
            modelFeatures: newModelFeatures);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsModelFeaturesUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.ModelFeatures);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, variant.Details.ModelFeatures);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_ModelFeaturesTheSame()
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
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act & Assert
        variant.ChangeDetailsModelFeatures(
            now: now,
            modelFeatures: variant.Details.ModelFeatures);
        
        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxModelFeaturesLength)]
    public void Should_Throw_Where_ModelFeaturesIsIncorrect(
        string modelFeatures)
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
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.ChangeDetailsModelFeatures(
                now: now,
                modelFeatures: modelFeatures));
    }
}