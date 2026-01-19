using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

public class ProductVariant
{
    private readonly List<ProductAttributeEntity> _attributes = new();
    private readonly List<ProductPriceHistoryEntity> _priceHistory = new();
    private readonly List<ProductImageEntity> _images = new();
    
    private readonly Color _color;
    private readonly ProductDetailEntity _productDetails;
    private readonly Product? _product;
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    public decimal Rating { get; private set; }
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
    public IReadOnlyCollection<ProductAttributeEntity> ProductAttributes => _attributes.AsReadOnly();
    public IReadOnlyCollection<ProductPriceHistoryEntity> PriceHistories => _priceHistory.AsReadOnly();
    public IReadOnlyCollection<ProductImageEntity> Images => _images.AsReadOnly();

    private const int MaxProductLength = 500;
    private const int MinProductLength = 25;
    
    private ProductVariant() { }

    public static ProductVariant Create(
        DateTimeOffset now,
        Guid productId,
        int colorId,
        string name,
        int inStock,
        string url)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
        
        if (colorId <= 0)
            throw new DomainException("Color Id cannot be less then 1.");

        var trimmedName = name.Trim();
        
        if(trimmedName.Length is < 25 or > 500)
            throw new DomainException("Product name must be < 500 and > 25.");
        
        if(inStock < 0)
            throw new DomainException("InStock cannot be less then 0.");

        string trimmedUrl = url.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmedUrl))
            throw new DomainException("Url cannot be string empty.");
        
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

    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted();
        
        string trimmedName = name.Trim();
        
        if (trimmedName == Name) return;
        
        if(trimmedName.Length is < 25 or > 500)
            throw new DomainException("Product name must be 15-500 characters.");
        
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
    
    public void UpdateRating(
        decimal sumOfRatings,
        decimal ratingsCount,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        Rating = sumOfRatings / ratingsCount;
        
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

    public void AddAttribute()
    {
        
    }
    
    public void RemoveAttribute()
    {
        
    }
    
    public void AddImage()
    {
        
    }

    public void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Product Variant already was deleted.");
    }
}