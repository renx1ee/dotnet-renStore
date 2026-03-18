/*using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class CreateTests
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
        var variantId = Guid.NewGuid();
        var countryId = 12;

        var trimmedDescription = description.Trim();
        var trimmedComposition = composition.Trim();

        // Act
        var detail = VariantDetail.Create(
            now: now,
            variantId: variantId,
            countryOfManufactureId: countryId,
            description: description,
            composition: composition,
            caringOfThings: caringOfThings,
            typeOfPackaging: typeOfPackaging,
            modelFeatures: modelFeatures,
            decorativeElements: decorativeElements,
            equipment: equipment);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var created = Assert.IsType<VariantDetailsCreated>(@event);

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
        Assert.NotNull(detail);
        Assert.NotEqual(Guid.Empty, detail.Id);
        Assert.Equal(created.Description, detail.Description);
        Assert.Equal(trimmedDescription, detail.Description);
        Assert.Equal(trimmedComposition, detail.Composition);
        Assert.Equal(caringOfThings, detail.CaringOfThings);
        Assert.Equal(typeOfPackaging, detail.TypeOfPacking);
        Assert.Equal(modelFeatures, detail.ModelFeatures);
        Assert.Equal(decorativeElements, detail.DecorativeElements);
        Assert.Equal(equipment, detail.Equipment);
        Assert.Equal(now, detail.CreatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IncorrectVariantId()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var variantId = Guid.Empty;

        // Act
        Assert.Throws<DomainException>(() => 
            VariantDetail.Create(
                now: now,
                variantId: variantId,
                countryOfManufactureId: 12,
                description: "Tests descriptiondescriptiondescrip",
                composition: "Composition fwwfwfwf",
                caringOfThings: null,
                typeOfPackaging: TypeOfPacking.Box,
                modelFeatures: null,
                decorativeElements: null,
                equipment: null));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Throw_Where_IncorrectCountryId(
        int countryId)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        // Act
        Assert.Throws<DomainException>(() => 
            VariantDetail.Create(
                now: now,
                variantId: Guid.NewGuid(),
                countryOfManufactureId: countryId,
                description: "Tests descriptiondescriptiondescrip",
                composition: "Composition fwwfwfwf",
                caringOfThings: null,
                typeOfPackaging: TypeOfPacking.Box,
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
        var variantId = Guid.NewGuid();
        var countryId = 12;

        // Act
        Assert.Throws<DomainException>(() => 
            VariantDetail.Create(
                now: now,
                variantId: variantId,
                countryOfManufactureId: countryId,
                description: description,
                composition: composition,
                caringOfThings: caringOfThings,
                typeOfPackaging: TypeOfPacking.Box,
                modelFeatures: modelFeatures,
                decorativeElements: decorativeElements,
                equipment: equipment));
    }
}*/