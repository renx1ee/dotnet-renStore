using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product cloth physical entity with lifecycle and invariants.
/// </summary>
public class ProductClothEntity
{
    private List<ProductClothSizeEntity> _clothSizes = new();
    private Product? _product;
    
    public Guid Id { get; private set; }
    public Gender? Gender { get; private set; }
    public Season? Season { get; private set; }
    public Neckline? Neckline { get; private set; }
    public TheCut? TheCut { get; private set; }
    public Guid ProductId { get; private set; }
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public IReadOnlyCollection<ProductClothSizeEntity>? ClothSizes => _clothSizes.AsReadOnly();
    
    private ProductClothEntity() { }

    public static ProductClothEntity Create(
        Gender? gender,
        Season? season,
        Neckline? neckline,
        TheCut? theCut,
        Guid productId,
        DateTimeOffset now)
    {
        ProductIdValidate(productId);
        
        var cloth = new ProductClothEntity()
        {
            ProductId = productId,
            IsDeleted = false,
            CreatedAt = now
        };

        if (gender.HasValue)
            cloth.Gender = gender;
        
        if (season.HasValue)
            cloth.Season = season;
        
        if (neckline.HasValue)
            cloth.Neckline = neckline;
        
        if (theCut.HasValue)
            cloth.TheCut = theCut;

        return cloth;
    }
    
    public static ProductClothEntity Reconstitute(
        Guid id,
        Gender? gender,
        Season? season,
        Neckline? neckline,
        TheCut? theCut,
        Guid productId,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        var cloth = new ProductClothEntity()
        {
            Id = id,
            Gender = gender,
            Season = season,
            Neckline = neckline,
            TheCut = theCut,
            ProductId = productId,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };

        return cloth;
    }
    
    public void ChangeGender(
        DateTimeOffset now,
        Gender gender)
    {
        EnsureNotDeleted();
        
        if(Gender == gender) return;

        Gender = gender;
        UpdatedAt = now;
    }
    
    public void ChangeSeason(
        DateTimeOffset now,
        Season season)
    {
        EnsureNotDeleted();
        
        if(Season == season) return;

        Season = season;
        UpdatedAt = now;
    }
    
    public void ChangeNeckLine(
        DateTimeOffset now,
        Neckline neckline)
    {
        EnsureNotDeleted();
        
        if(Neckline == neckline) return;

        Neckline = neckline;
        UpdatedAt = now;
    }
    
    public void ChangeTheCut(
        DateTimeOffset now,
        TheCut theCut)
    {
        EnsureNotDeleted();
        
        if(TheCut == theCut) return;

        TheCut = theCut;
        UpdatedAt = now;
    }
    
    public void Delete(DateTimeOffset now)
    {
        if(IsDeleted)
            throw new DomainException("Product Cloth already was deleted.");

        IsDeleted = true;
        UpdatedAt = now;
        DeletedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException("Product Cloth is not deleted.");
        
        IsDeleted = false;
        
        DeletedAt = null;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted()
    {
        if(IsDeleted)
            throw new DomainException("Product Cloth already was deleted.");
    }

    private static void ProductIdValidate(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
    }
}