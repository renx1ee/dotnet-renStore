using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeDetailsEquipmentTests : ProductVariantTestBase
{
    private const string MaxEquipmentLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New Equipment")]
    [InlineData(" New Equipment ")]
    public void Should_Raise_EquipmentUpdated_Event(
        string newEquipment)
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
            typeOfPacking: TypeOfPacking.Box,
            modelFeatures: "model features features features features",
            decorativeElements: "decorative elements elements elements elements",
            equipment: "equipment equipment equipment equipment equipment equipment");
        
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeDetailsEquipment(
            now: now,
            equipment: newEquipment);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsEquipmentUpdatedEvent>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Equipment);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, variant.Details.Equipment);
        Assert.Equal(now, variant.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_EquipmentTheSame()
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
        variant.ChangeDetailsEquipment(
            now: now,
            equipment: variant.Details.Equipment);

        Assert.Empty(variant.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxEquipmentLength)]
    public void Should_Throw_Where_EquipmentIsIncorrect(
        string equipment)
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
            variant.ChangeDetailsEquipment(
                now: now,
                equipment: equipment));
    }
}