using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsDescriptionTests : ProductVariantTestBase
{
    private const string MaxDescriptionLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New sample sample Description")]
    [InlineData(" New sample sample Description ")]
    public void Should_Raise_DescriptionUpdated_Event(
        string newDescription)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New sample sample Description";
        
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
        variant.ChangeDetailsDescription(
            now: now,
            description: newDescription);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsDescriptionUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Description);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, variant.Details.Description);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_DescriptionTheSame()
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
        variant.ChangeDetailsDescription(
            now: now,
            description: variant.Details.Description);
        
        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New Description")]
    [InlineData(MaxDescriptionLength)]
    public void Should_Throw_Where_DescriptionIsIncorrect(
        string newDescription)
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
            variant.ChangeDetailsDescription(
                now: now,
                description: newDescription));
    }
}