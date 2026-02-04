using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product cloth physical entity with lifecycle and invariants.
/// </summary>
public class ProductCloth
    : RenStore.Catalog.Domain.Entities.EntityWithSoftDeleteBase
{
    private List<ProductClothSize> _clothSizes = new();
    private Product? _product;
    
    public Guid Id { get; private set; }
    public Gender? Gender { get; private set; }
    public Season? Season { get; private set; }
    public Neckline? Neckline { get; private set; }
    public TheCut? TheCut { get; private set; }
    public Guid ProductId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public IReadOnlyCollection<ProductClothSize>? ClothSizes => _clothSizes.AsReadOnly();
    
    private ProductCloth() { }

    public static ProductCloth Create(
        Gender? gender,
        Season? season,
        Neckline? neckline,
        TheCut? theCut,
        Guid productId,
        DateTimeOffset now)
    {
        ProductIdValidate(productId);
        
        var cloth = new ProductCloth()
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
    
    public static ProductCloth Reconstitute(
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
        var cloth = new ProductCloth()
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

    private static void ProductIdValidate(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
    }
}