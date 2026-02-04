using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public class ProductVariant
    : RenStore.Catalog.Domain.Entities.EntityWithSoftDeleteBase
{
    private readonly List<ProductAttribute> _attributes = new();
    private readonly List<ProductPriceHistory> _priceHistory = new();
    private readonly List<ProductImage> _images = new();
    
    private readonly Product _product;
    
    private Color _color;
    private ProductDetail _productDetails;
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    public Rating? Rating { get; private set; } // TODO: Rating
    public long Article { get; private set; } // TODO: понять где лучше создавать артикул
    public int InStock { get; private set; }
    public int Sales { get; private set; }
    public ProductVariantStatus Status { get; private set; }
    public bool IsAvailable { get; private set; }  
    public string Url { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid ProductId { get; private set; }
    public int ColorId { get; private set; }
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
            IsDeleted = isDeleted,
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
        
        var variant = new ProductVariant()
        {
            ProductId = productId,
            Name = trimmedName,
            NormalizedName = trimmedName.ToUpperInvariant(),
            InStock = inStock,
            Url = trimmedUrl,
            CreatedAt = now,
            IsDeleted = false,
            IsAvailable = false
        };

        return variant;
    }
    
    public void Publish(DateTimeOffset now)
    {
        if (!_images.Any())
            throw new DomainException("Variant must have images.");
        
        if (_productDetails == null)
            throw new DomainException("Variant must have details.");

        Status = ProductVariantStatus.Published;
        SetAvailability(true, now); // TODO: убедится
        UpdatedAt = now;
    }
    
    public void AddDetails(
        DateTimeOffset now,
        int countryOfManufactureId,
        Guid productVariantId,
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
        
        var detail = ProductDetail.Create(
            now: now,
            countryOfManufactureId: countryOfManufactureId,
            productVariantId: productVariantId,
            description: description,
            modelFeatures: modelFeatures,
            decorativeElements: decorativeElements,
            equipment: equipment,
            composition: composition,
            caringOfThings: caringOfThings,
            typeOfPackaging: typeOfPackaging);
        
        _productDetails = detail;
        UpdatedAt = now;
    }
    
    public void AddAttribute(
        DateTimeOffset now,
        string key,
        string value)
    {
        EnsureNotDeleted();
        
        if (_attributes.Count >= MaxAttributesCount)
            throw new DomainException($"Attributes count must be less then {MaxAttributesCount}.");
        
        var attribute = ProductAttribute.Create(
            key: key,
            value: value,
            productVariantId: Id,
            now: now);

        _attributes.Add(attribute);
        UpdatedAt = now;
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
        if (_images.Count >= MaxImagesCount)
            throw new DomainException($"Product images count must be less then {MaxImagesCount}.");
        
        var image = ProductImage.Create(
            now: now,
            productVariantId: Id,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            isMain: isMain,
            sortOrder: sortOrder,
            weight: weight,
            height: height);

        if (isMain)
            MarkImageAsMain(now, image);

        UpdatedAt = now;
        _images.Add(image);
    }
    
    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted();
        
        string trimmedName = name.Trim();
        
        if (trimmedName == Name) return;
        
        ValidateName(trimmedName);

        Name = trimmedName;
        NormalizedName = trimmedName.ToUpperInvariant();
        UpdatedAt = now;
    }
    
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");

        InStock += count;
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

        InStock -= count;

        if (InStock <= 0)
            IsAvailable = false;

        UpdatedAt = now;
    }

    public void Sell(
        DateTimeOffset now,
        int count)
    {
        RemoveFromStock(now, count);
        Sales += count;
    }
    // TODO:
    public void UpdateRating(
        decimal sumOfRatings,
        decimal ratingsCount,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        // TODO:
        
        UpdatedAt = now;
    }
    
    public void SetAvailability(
        bool isAvailable,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if(IsAvailable == isAvailable) return;
        
        IsAvailable = isAvailable;
        UpdatedAt = now;
    }
    
    public void MarkImageAsMain(
        DateTimeOffset now,
        ProductImage image)
    {
        var mainImage = _images.FirstOrDefault(x => x.IsMain);

        if (mainImage != null)
            mainImage.UnsetAsMain(now);
        
        image.SetAsMain(now);
    }
    
    public void DeleteAttribute(
        DateTimeOffset now,
        Guid attributeId)
    {
        var attribute = _attributes.FirstOrDefault(x => x.Id == attributeId);

        if (attribute == null)
            throw new DomainException("Cannot delete not exists variant.");
        
        // TODO: сделать правильно 
        /*attribute.Restore(now);*/
        _attributes.Remove(attribute);
        
        UpdatedAt = now;
    }
    //TODO: сделать проверку, если есть всего изображение, можно ли его удалять
    public void DeleteImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var result = _images.FirstOrDefault(x => x.Id == imageId);

        if (result == null)
            throw new DomainException("Product image not exists.");
        
        // TODO: сделать правильно
        /*result.Delete(now);*/
        _images.Remove(result);
        UpdatedAt = now;
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