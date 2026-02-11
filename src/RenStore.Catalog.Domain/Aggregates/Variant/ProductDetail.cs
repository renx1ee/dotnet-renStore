using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
internal class ProductDetail
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } 
    public string? ModelFeatures { get; private set; }
    public string? DecorativeElements { get; private set; } 
    public string? Equipment { get; private set; }
    public string Composition { get; private set; }
    public string? CaringOfThings { get; private set; } 
    public TypeOfPackaging? TypeOfPacking { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public bool IsDeleted { get; private set; } // TODO: проверить, нужно ли вообще
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private ProductDetail() { }

    internal static ProductDetail Create(
        Guid id,
        DateTimeOffset now,
        int countryOfManufactureId,
        Guid productVariantId,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPackaging? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        return new ProductDetail()
        {
            Id = id,
            Description = description, 
            CreatedAt = now,
            CountryOfManufactureId = countryOfManufactureId,
            ProductVariantId = productVariantId,
            Composition = composition,
            CaringOfThings = caringOfThings,
            TypeOfPacking = typeOfPackaging,
            ModelFeatures = modelFeatures,
            DecorativeElements = decorativeElements,
            Equipment = equipment,
            IsDeleted = false
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
        TypeOfPackaging typeOfPackaging)
    {
        TypeOfPacking = typeOfPackaging;
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

/*
 * public static ProductDetail Reconstitute(
       Guid id,
       int countryOfManufactureId,
       Guid productVariantId,
       string description,
       string modelFeatures,
       string decorativeElements,
       string equipment,
       string composition,
       string caringOfThings,
       TypeOfPackaging? typeOfPackaging,
       bool isDeleted,
       DateTimeOffset createdAt,
       DateTimeOffset updatedAt,
       DateTimeOffset deletedAt)
   {
       var detail = new ProductDetail()
       {
           Id = id,
           Description = description,
           ModelFeatures = modelFeatures,
           DecorativeElements = decorativeElements,
           Equipment = equipment,
           Composition = composition,
           CaringOfThings = caringOfThings,
           TypeOfPacking = typeOfPackaging,
           CountryOfManufactureId = countryOfManufactureId,
           ProductVariantId = productVariantId,
           IsDeleted = isDeleted,
           CreatedAt = createdAt,
           UpdatedAt = updatedAt,
           DeletedAt = deletedAt,
       };

       return detail;
   }
 */