using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;

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
    public CountryOfManufacture CountryOfManufacture { get; private set; }
    public Guid VariantId { get; private set; }
    
    private VariantDetail() { }

    internal static VariantDetail Create(
        Guid detailId,
        DateTimeOffset now,
        Guid variantId,
        string description,
        string composition,
        string countryOfManufacture,
        string? caringOfThings = null,
        TypeOfPacking? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        return new VariantDetail()
        {
            Id = detailId,
            CreatedAt = now,
            VariantId = variantId,
            CountryOfManufacture = new CountryOfManufacture(countryOfManufacture),
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
        string decorativeElements)
    {
        DecorativeElements = decorativeElements;
        UpdatedAt = now;
    }

    internal void ChangeEquipment(
        DateTimeOffset now,
        string equipment)
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
        string caringOfThings)
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

    internal void ChangeCountryOfManufacture(
        DateTimeOffset now,
        string countryOfManufacture)
    {
        CountryOfManufacture = new CountryOfManufacture(countryOfManufacture);
        UpdatedAt = now;
    }
}