using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public class ProductVariant
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<ProductAttribute> _attributes = new();
    private readonly List<ProductPriceHistory> _priceHistory = new();
    private readonly List<ProductImage> _images = new();
    
    private Color _color;
    private ProductDetail _productDetails;
    
    public Guid Id { get; private set; }
    /*public ProductName Name { get; private set; }*/
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    public Rating Rating { get; private set; } 
    public long Article { get; private set; } // TODO: понять где лучше создавать артикул
    /*public Article Article { get; private set; } */
    public int InStock { get; private set; }
    public int Sales { get; private set; }
    public ProductVariantStatus Status { get; private set; }
    public bool IsAvailable { get; private set; }  
    public string Url { get; private set; }
    /*public Url Url { get; private set; }*/
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid ProductId { get; private set; }
    public int ColorId { get; private set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    
    public IReadOnlyCollection<ProductAttribute> ProductAttributes => _attributes.AsReadOnly();
    public IReadOnlyCollection<ProductPriceHistory> PriceHistories => _priceHistory.AsReadOnly();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    private const int MaxProductNameLength = 500;
    private const int MinProductNameLength = 25;
    
    private const int MaxImagesCount       = 50;
    private const int MaxAttributesCount   = 50;
    
    private ProductVariant() { }
    
    public static ProductVariant Reconstitute(
        Guid id,
        Guid productId,
        int colorId,
        string name,
        string normalizedName,
        Rating rating,
        long article,
        int inStock,
        int sales,
        bool isAvailable,
        string url,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? uploadAt,
        DateTimeOffset? deletedAt)
    {
        var variant = new ProductVariant()
        {
            Id = id,
            ProductId = productId,
            ColorId = colorId,
            Name = name,
            NormalizedName = normalizedName,
            Rating = rating,
            Article = article,
            Sales = sales,
            IsAvailable = isAvailable,
            InStock = inStock,
            Url = url,
            CreatedAt = createdAt,
            UpdatedAt = uploadAt,
            DeletedAt = deletedAt
        };

        return variant;
    }
    
    public static ProductVariant Create(
        DateTimeOffset now,
        Guid productId,
        int colorId,
        string name,
        int inStock,
        string url)
    {
        ValidateProductId(productId);

        ValidateColorId(colorId);

        var trimmedName = name.Trim();

        ValidateName(trimmedName);
        
        ValidateInStock(inStock);

        string trimmedUrl = url.Trim();

        ValidateUrl(trimmedUrl);
        
        var variant = new ProductVariant();
        var variantId = Guid.NewGuid();
        
        variant.Raise(
            new VariantCreated(
                VariantId: variantId,
                ProductId: productId,
                ColorId: colorId,
                Name: trimmedName,
                InStock: inStock,
                Url: trimmedUrl,
                OccurredAt: now));
        
        return variant;
    }
    
    public void AddDetails(
        DateTimeOffset now,
        int countryOfManufactureId,
        string description,
        string modelFeatures,
        string decorativeElements,
        string equipment,
        string composition,
        string caringOfThings,
        TypeOfPackaging? typeOfPackaging = null)
    {
        EnsureNotDeleted();
        
        if (_productDetails != null)
            throw new DomainException("Product details already was created!");

        ProductDetailRules.CountryOfManufactureValidate(countryOfManufactureId);
        ProductDetailRules.ProductVariantIdValidate(Id);

        var trimmedDescription              = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        var trimmedComposition        = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        var trimmedModelFeatures      = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        var trimmedEquipment          = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        var trimmedCaringOfThings     = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);

        var detailId = Guid.NewGuid();
        
        Raise(new VariantDetailsCreated(
            OccurredAt: now,
            Id: detailId,
            VariantId: Id,
            CountryOfManufactureId: countryOfManufactureId,
            ModelFeatures: trimmedModelFeatures ?? null,
            DecorativeElements: trimmedDecorativeElements ?? null,
            Equipment: trimmedEquipment ?? null,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings ?? null,
            TypeOfPackaging: typeOfPackaging ?? null));
    }
    
    public void AddAttribute(
        DateTimeOffset now,
        string key,
        string value)
    {
        EnsureNotDeleted();
        
        if (_attributes.Count >= MaxAttributesCount)
            throw new DomainException($"Attributes count must be less then {MaxAttributesCount}.");
        
        ProductAttributeRules.ProductVariantIdNormalizeAndValidate(Id);
        
        var trimmedKey   = ProductAttributeRules.KeyNormalizeAndValidate(key);
        var trimmedValue = ProductAttributeRules.ValueNormalizeAndValidate(value);
        
        Raise(new VariantAttributeCreated(
            VariantId: Id,
            OccurredAt: now,
            Key: trimmedKey,
            Value: trimmedValue));
    }
    
    public void AddImage(
        DateTimeOffset now,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        EnsureNotDeleted();
        
        if (_images.Count >= MaxImagesCount)
            throw new DomainException($"Product images count must be less then {MaxImagesCount}.");

        var imageId = Guid.NewGuid();
        
        ProductImageRules.CreateProductImageValidation(
            imageId: imageId,
            productVariantId: Id,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            sortOrder: sortOrder,
            weight: weight,
            height: height);
        
        if (isMain)
        {
            var currentMain = _images.FirstOrDefault(x => x.IsMain);

            if (currentMain != null)
            {
                Raise(new VariantImageMainUnset(
                    OccurredAt: now,
                    VariantId: Id,
                    ImageId: currentMain.Id));
            }
        }
        
        Raise(new VariantImageCreated(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            OriginalFileName: originalFileName,
            StoragePath: storagePath,
            FileSizeBytes: fileSizeBytes,
            IsMain: isMain,
            SortOrder: sortOrder,
            Weight: weight,
            Height: height));
    }
    // TODO:
    public void AddColor()
    {
        EnsureNotDeleted();
    }
    // TODO:
    public void ChangeColor()
    {
        EnsureNotDeleted();
    }
    
    public void Publish(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (!_images.Any())
            throw new DomainException("Variant must have images.");
        
        if (_productDetails == null)
            throw new DomainException("Variant must have details.");
        
        Raise(new VariantPublished(
            VariantId: Id,
            OccurredAt: now));
    }
    
    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted();
        
        string trimmedName = name.Trim();
        
        if (trimmedName == Name) return;
        
        ValidateName(trimmedName);

        Raise(new VariantNameUpdated(
            OccurredAt: now,
            VariantId: Id,
            Name: trimmedName));
    }
    
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");

        Raise(new VariantStockAdded(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }
    
    public void RemoveFromStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        if (IsAvailable && count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
        
        if(count > InStock)
            throw new DomainException("The count of sells exceed available count.");

        Raise(new VariantRemovedFromStock(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }

    public void Sell(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();
        
        Raise(new SaleOfVariantOccurred(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }
    
    public void UpdateRating(
        decimal score,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new VariantAverageRatingUpdated(
            VariantId: Id,
            OccurredAt: now,
            Score: score));
    }
    
    public void SetAvailability(
        bool isAvailable,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if(IsAvailable == isAvailable) return;
        
        Raise(new VariantAvailabilityUpdated(
            OccurredAt: now,
            VariantId: Id,
            IsAvailable: isAvailable));
    }
    
    public void MarkImageAsMain(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        var currentImage = _images.FirstOrDefault(x => x.Id == imageId);

        if (currentImage == null)
            throw new DomainException("Image was not found.");
        
        if(currentImage.IsDeleted)
            throw new DomainException("Cannot mark already deleted image as main.");
        
        var currentMain = _images.FirstOrDefault(x => x.IsMain);
        
        if(currentMain?.Id == imageId)
            return;

        if (currentMain != null)
        {
            Raise(new VariantImageMainUnset(
                OccurredAt: now,
                VariantId: Id,
                ImageId: currentMain.Id));
        }
        
        Raise(new VariantImageMainUnset(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    public void DeleteAttribute(
        DateTimeOffset now,
        Guid attributeId)
    {
        var attribute = _attributes.FirstOrDefault(x => x.Id == attributeId);

        if (attribute == null)
            throw new DomainException("Cannot delete not exists variant.");
        
        
        Raise(new VariantAttributeRemoved(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
        // TODO: Raise
        /*attribute.Restore(e.OccurredAt);
        _attributes.Remove(attribute);
        UpdatedAt = e.OccurredAt;*/
    }
    //TODO: сделать проверку, если есть всего изображение, можно ли его удалять
    public void DeleteImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var result = _images.FirstOrDefault(x => x.Id == imageId);

        if (result == null)
            throw new DomainException("Product image not exists.");
        
        if (result.IsDeleted)
            throw new DomainException("The image already was deleted.");
    }

    public void RestoreImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var result = _images.FirstOrDefault(x => x.Id == imageId);

        if (result == null)
            throw new DomainException("Product image not exists.");
        
        if (!result.IsDeleted)
            throw new DomainException("The image was not deleted.");
        
        
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case VariantCreated e:
                CreatedAt = e.OccurredAt;
                Id = e.VariantId;
                ProductId = e.ProductId;
                ColorId = e.ColorId;
                Name = e.Name;
                NormalizedName = e.Name.ToUpperInvariant();
                InStock = e.InStock;
                Url = e.Url;
                IsAvailable = false;
                break;
            
            case VariantDetailsCreated e:
                _productDetails = ProductDetail.Create(
                    id: e.Id,
                    now: e.OccurredAt,
                    countryOfManufactureId: e.CountryOfManufactureId,
                    productVariantId: Id,
                    description: e.Description,
                    composition: e.Composition,
                    caringOfThings: e.CaringOfThings,
                    typeOfPackaging: e.TypeOfPackaging, 
                    modelFeatures: e.ModelFeatures, 
                    decorativeElements: e.DecorativeElements, 
                    equipment: e.Equipment);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeCreated e:
                _attributes.Add(ProductAttribute.Create(
                    key: e.Key,
                    value: e.Value,
                    productVariantId: Id,
                    now: e.OccurredAt));
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageCreated e:
                _images.Add(ProductImage.Create(
                    now: e.OccurredAt,
                    imageId: e.ImageId,
                    productVariantId: Id,
                    originalFileName: e.OriginalFileName,
                    storagePath: e.StoragePath,
                    fileSizeBytes: e.FileSizeBytes,
                    isMain: e.IsMain,
                    sortOrder: e.SortOrder,
                    weight: e.Weight,
                    height: e.Height));
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantPublished e:
                Status = ProductVariantStatus.Published;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantNameUpdated e:
                Name = e.Name;
                NormalizedName = e.Name.ToUpperInvariant();
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantStockAdded e:
                InStock += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantRemovedFromStock e:
                InStock -= e.Count;

                if (InStock <= 0)
                    IsAvailable = false;
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SaleOfVariantOccurred e:
                RemoveFromStock(e.OccurredAt, e.Count);
                Sales += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAverageRatingUpdated e:
                Rating = Rating.Add(e.Score);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAvailabilityUpdated e:
                IsAvailable = e.IsAvailable;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageMainUnset e:
                _images.Single(x => x.Id == e.ImageId)
                    .UnsetAsMain(e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            case VariantImageMainSet e:
                _images.Single(x => x.Id == e.ImageId)
                    .SetAsMain(e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            // TODO:
            case VariantAttributeRemoved e:
                UpdatedAt = e.OccurredAt;
                break;
            // TODO:
            case VariantImageRemoved e:
                var result = _images.Single(x => x.Id == e.ImageId);
                result.Delete(e.OccurredAt); // TODO:
                _images.Remove(result);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ProductVariantStatus.IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
    
    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
    }
    
    private static void ValidateColorId(int colorId)
    {
        if (colorId <= 0)
            throw new DomainException("Color Id cannot be less then 1.");
    }
    
    private static void ValidateName(string name)
    {
        if(name.Length is < MinProductNameLength or > MaxProductNameLength)
            throw new DomainException("Product name must be < 500 and > 25.");
    }
    
    private static void ValidateUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Url cannot be string empty.");
    }
    
    private static void ValidateInStock(int inStock)
    {
        if(inStock < 0)
            throw new DomainException("InStock cannot be less then 0.");
    }
}

// TODO: сделать создание
//  1) цвета, истории цен,
//  2) удаление аттрибутов (в самих аттрибутах убрать исключения, и перенести их в вариант методы перед Apply)