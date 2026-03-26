using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class AddDetailsTests : ProductVariantTestBase
{
    [Theory]
    [InlineData(" Test description text text text", " CompositionCompositionCompositionComposition", "caring of things things v things things", TypeOfPacking.Box, "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("Test description text text text ", "CompositionCompositionCompositionComposition ", null, null, null, null, null)]
    public void Should_Raise_Created_Event(
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPacking? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var countryId = 12;

        var trimmedDescription = description.Trim();
        var trimmedComposition = composition.Trim();
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.AddDetails(
            now: now,
            countryOfManufactureId: countryId,
            description: description,
            composition: composition,
            caringOfThings: caringOfThings,
            typeOfPacking: typeOfPackaging,
            modelFeatures: modelFeatures,
            decorativeElements: decorativeElements,
            equipment: equipment);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var created = Assert.IsType<VariantDetailsCreatedEvent>(@event);

        // Assert: event
        Assert.Equal(trimmedDescription, created.Description);
        Assert.Equal(trimmedComposition, created.Composition);
        Assert.Equal(caringOfThings, created.CaringOfThings);
        Assert.Equal(typeOfPackaging, created.TypeOfPackaging);
        Assert.Equal(modelFeatures, created.ModelFeatures);
        Assert.Equal(decorativeElements, created.DecorativeElements);
        Assert.Equal(equipment, created.Equipment);
        Assert.Equal(now, created.OccurredAt);
        
        // Assert: state
        Assert.NotNull(variant.Details);
        Assert.NotEqual(Guid.Empty, variant.Details.Id);
        Assert.Equal(created.Description, variant.Details.Description);
        Assert.Equal(trimmedDescription, variant.Details.Description);
        Assert.Equal(trimmedComposition, variant.Details.Composition);
        Assert.Equal(caringOfThings, variant.Details.CaringOfThings);
        Assert.Equal(typeOfPackaging, variant.Details.TypeOfPacking);
        Assert.Equal(modelFeatures, variant.Details.ModelFeatures);
        Assert.Equal(decorativeElements, variant.Details.DecorativeElements);
        Assert.Equal(equipment, variant.Details.Equipment);
        Assert.Equal(now, variant.Details.CreatedAt);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Throw_Where_IncorrectCountryId(
        int countryId)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        Assert.Throws<DomainException>(() => 
            variant.AddDetails(
                now: now,
                countryOfManufactureId: countryId,
                description: "Tests descriptiondescriptiondescrip",
                composition: "Composition fwwfwfwf",
                caringOfThings: null,
                typeOfPacking: TypeOfPacking.Box,
                modelFeatures: null,
                decorativeElements: null,
                equipment: null));
    }
    
    [Theory]
    [InlineData("Test", "CompositionCompositionCompositionComposition", "caring of things things v things things", "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("", "CompositionCompositionCompositionComposition", "caring of things things v things things", "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData(" ", "CompositionCompositionCompositionComposition", "caring of things things v things things", "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("TestTest TestTest TestTestTest", "Comp", "caring of things things v things things", "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("TestTest TestTest TestTestTest", "CompositionCompositionCompositionComposition", "car", "model features features features features", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("TestTest TestTest TestTestTest", "CompositionCompositionCompositionComposition", "caring of things things v things things", "mod", "decorative elements elements elements elements", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("TestTest TestTest TestTestTest", "CompositionCompositionCompositionComposition", "caring of things things v things things", "model features features features features", "dec", "equipment equipment equipment equipment equipment equipment")]
    [InlineData("TestTest TestTest TestTestTest", "CompositionCompositionCompositionComposition", "caring of things things v things things", "model features features features features", "decorative elements elements elements elements", "eq")]
    public void Should_Throw_Where_ParametersAreWrong(
        string description,
        string composition,
        string? caringOfThings = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var countryId = 12;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        Assert.Throws<DomainException>(() => 
            variant.AddDetails(
                now: now,
                countryOfManufactureId: countryId,
                description: description,
                composition: composition,
                caringOfThings: caringOfThings,
                typeOfPacking: TypeOfPacking.Box,
                modelFeatures: modelFeatures,
                decorativeElements: decorativeElements,
                equipment: equipment));
    }
}