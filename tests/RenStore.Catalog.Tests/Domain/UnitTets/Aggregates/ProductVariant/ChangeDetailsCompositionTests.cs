using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsCompositionTests : ProductVariantTestBase
{
    private const string MaxCompositionLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New Composition")]
    [InlineData(" New Composition ")]
    public void Should_Raise_CompositionUpdated_Event(
        string newComposition)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New Composition";

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
        variant.ChangeDetailsComposition(
            now: now,
            composition: newComposition);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCompositionUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Composition);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, variant.Details.Composition);
        Assert.Equal(now, variant.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_CompositionTheSame()
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
        variant.ChangeDetailsComposition(
            now: now,
            composition: variant.Details.Composition);

        Assert.Empty(variant.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxCompositionLength)]
    public void Should_Throw_Where_CompositionIsIncorrect(
        string composition)
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
            variant.ChangeDetailsComposition(
                now: now,
                composition: composition));
    }
}