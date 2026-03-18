using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

public sealed class VariantDetail
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } 
    public string Composition { get; private set; }
    public string? ModelFeatures { get; private set; }
    public string? DecorativeElements { get; private set; } 
    public string? Equipment { get; private set; }
    public string? CaringOfThings { get; private set; } 
    public TypeOfPacking? TypeOfPacking { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public Guid VariantId { get; private set; }
    
    private VariantDetail() { }

    internal static VariantDetail Create(
        Guid detailId,
        DateTimeOffset now,
        Guid variantId,
        int countryOfManufactureId,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPacking? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        return new VariantDetail()
        {
            CreatedAt = now,
            VariantId = variantId,
            CountryOfManufactureId = countryOfManufactureId,
            ModelFeatures = modelFeatures,
            DecorativeElements = decorativeElements,
            Equipment = equipment,
            Description = description,
            Composition = composition,
            CaringOfThings = caringOfThings,
            TypeOfPacking = typeOfPackaging
        };
    }
    
    internal void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        Description = description;
        UpdatedAt = now;
    }

    internal void ChangeModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        ModelFeatures = modelFeatures;
        UpdatedAt = now;
    }

    internal void ChangeDecorativeElements(
        DateTimeOffset now,
        string? decorativeElements)
    {
        DecorativeElements = decorativeElements;
        UpdatedAt = now;
    }

    internal void ChangeEquipment(
        DateTimeOffset now,
        string? equipment)
    {
        Equipment = equipment;
        UpdatedAt = now;
    }

    internal void ChangeComposition(
        DateTimeOffset now,
        string composition)
    {
        Composition = composition;
        UpdatedAt = now;
    }

    internal void ChangeCaringOfThings(
        DateTimeOffset now,
        string? caringOfThings)
    {
        CaringOfThings = caringOfThings;
        UpdatedAt = now;
    }
    
    internal void ChangeTypeOfPacking(
        DateTimeOffset now,
        TypeOfPacking typeOfPacking)
    {
        TypeOfPacking = typeOfPacking;
        UpdatedAt = now;
    }

    internal void ChangeCountryOfManufactureId(
        DateTimeOffset now,
        int countryOfManufactureId)
    {
        CountryOfManufactureId = countryOfManufactureId;
        UpdatedAt = now;
    }
}