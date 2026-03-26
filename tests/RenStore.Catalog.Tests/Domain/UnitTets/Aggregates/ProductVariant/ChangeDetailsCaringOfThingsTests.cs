using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsCaringOfThingsTests : ProductVariantTestBase
{
    private const string MaxCaringOfThingsLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New CaringOfThings")]
    [InlineData(" New CaringOfThings ")]
    public void Should_Raise_CaringOfThingsUpdated_Event(
        string newCaringOfThings)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New CaringOfThings";

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
        variant.ChangeDetailsCaringOfThings(
            now: now,
            caringOfThings: newCaringOfThings);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCaringOfThingsUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.CaringOfThings);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, variant.Details.CaringOfThings);
        Assert.Equal(now, variant.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_CaringOfThingsTheSame()
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
        variant.ChangeDetailsCaringOfThings(
            now: now,
            caringOfThings: variant.Details!.CaringOfThings!);

        Assert.Empty(variant.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxCaringOfThingsLength)]
    public void Should_Throw_Where_CaringOfThingsIsIncorrect(
        string caringOfThings)
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
            variant.ChangeDetailsCaringOfThings(
                now: now,
                caringOfThings: caringOfThings));
    }
}