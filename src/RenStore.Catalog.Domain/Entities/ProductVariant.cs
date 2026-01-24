using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public class ProductVariant
{
    private readonly List<ProductAttribute> _attributes = new();
    private readonly List<ProductPriceHistoryEntity> _priceHistory = new();
    private readonly List<ProductImageEntity> _images = new();
    
    private readonly Color _color;
    private readonly ProductDetailEntity _productDetails;
    private readonly Product? _product;
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    // TODO:
    public Rating Rating { get; private set; }
    public long Article { get; private set; }
    public int InStock { get; private set; }
    public int Sales { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public string Url { get; private set; }
    public Guid ProductId { get; private set; }
    public int ColorId { get; private set; }
    public IReadOnlyCollection<ProductAttribute> ProductAttributes => _attributes.AsReadOnly();
    public IReadOnlyCollection<ProductPriceHistoryEntity> PriceHistories => _priceHistory.AsReadOnly();
    public IReadOnlyCollection<ProductImageEntity> Images => _images.AsReadOnly();

    #region Main
    
    private const int MaxProductNameLength = 500;
    private const int MinProductNameLength = 25;
    
    private ProductVariant() { }

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
            IsDeleted = false
        };

        return variant;
    }

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

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted product variant.");
        
        IsDeleted = true;
        IsAvailable = false;
        
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if (!IsDeleted)
            throw new DomainException("The product variant is not deleted!");
        
        IsDeleted = false;
        IsAvailable = true;
        
        DeletedAt = null;
        UpdatedAt = now;
    }

    public void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Product Variant already was deleted.");
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
    
    #endregion

    #region Attributes

    public void AddAttributeToVariant(
        DateTimeOffset now,
        string key,
        string value)
    {
        var attribute = ProductAttribute.Create(
            key: key,
            value: value,
            productVariantId: Id,
            now: now);

        UpdatedAt = now;
        
        _attributes.Add(attribute);
    }

    public void ChangeAttributeKeyToVariant()
    {
        
    }
    
    public void RemoveAttribute()
    {
        
    }
    
    

    #endregion
    
    #region Details

    

    #endregion
    
    #region Images

    public void AddImage()
    {
        
    }

    #endregion
}