using RenStore.Catalog.Domain.Aggregates.Category.Events;
using RenStore.Catalog.Domain.Aggregates.Category.Rules;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Category;

/// <summary>
/// Represents a category physical entity with lifecycle and invariants.
/// </summary>
public sealed class Category 
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<SubCategory> _subCategories = new();
    
    public Guid Id { get; private set; }
    public string Name { get; private set; } 
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; } 
    public string NormalizedNameRu { get; private set; } 
    public string? Description { get; private set; }
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    public bool IsActive { get; private set; } 
    public bool IsDeleted { get; private set; } 
    public DateTimeOffset CreatedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; } 
    public DateTimeOffset? DeletedAt { get; private set; } 
    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();
    
    private Category() { }
    
    public static Category Create(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string name,
        string nameRu,
        bool isActive,
        string? description = null)
    {
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var trimmedName = CategoryRules.NormalizeAndValidateName(name);
        var trimmedNameRu = CategoryRules.NormalizeAndValidateNameRu(nameRu);
        var trimmedDescription = CategoryRules.NormalizeAndValidateDescription(description);
        var upperName = trimmedName.ToUpperInvariant();
        var upperNameRu = trimmedNameRu.ToUpperInvariant();

        var categoryId = Guid.NewGuid();

        var category = new Category();
        
        category.Raise(new CategoryCreatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: categoryId,
            Name: trimmedName,
            IsActive: isActive,
            NormalizedName: upperName,
            NameRu: trimmedNameRu,
            NormalizedNameRu: upperNameRu,
            Description: trimmedDescription));
        
        return category;
    }

    public void ChangeName(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var trimmedName = CategoryRules.NormalizeAndValidateName(name);
        
        if (trimmedName == Name) return;

        var normalizedName = trimmedName.ToUpperInvariant();
        
        Raise(new CategoryNameChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            Name: trimmedName,
            NormalizedName: normalizedName));
    }
    
    public void ChangeNameRu(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var trimmedNameRu = CategoryRules.NormalizeAndValidateNameRu(nameRu);
        
        if (trimmedNameRu == NameRu) return;
        
        var normalizedNameRu = trimmedNameRu.ToUpperInvariant();
        
        Raise(new CategoryNameRuChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            NameRu: trimmedNameRu,
            NormalizedNameRu: normalizedNameRu));
    }
    
    public void ChangeDescription( 
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var trimmedDescription = CategoryRules.NormalizeAndValidateDescription(description);
        
        if (trimmedDescription == Description) return;
        
        Raise(new CategoryDescriptionChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            Description: trimmedDescription));
    }

    public void Activate(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);

        if (IsActive) return;
        
        Raise(new CategoryActivatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id));
    }
    
    public void Deactivate(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        if (!IsActive) return;
        
        Raise(new CategoryDeactivatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        if (IsDeleted) return;

        foreach (var subCategory in _subCategories.Where(x => !x.IsDeleted))
        {
            Raise(new SubCategoryDeletedEvent(
                UpdatedById: updatedById,
                UpdatedByRole: updatedByRole,
                EventId: Guid.NewGuid(),
                OccurredAt: now,
                SubCategoryId: subCategory.Id,
                CategoryId: Id));
        }
        
        Raise(new CategoryDeletedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id));
    }
    
    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        if (!IsDeleted) return;
        
        Raise(new CategoryRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id));
    }

    public Guid CreateSubCategory(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string name,
        string nameRu,
        bool isActive,
        string? description = null)
    {
        EnsureNotDeleted();

        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);

        var trimmedName = CategoryRules.NormalizeAndValidateName(name);
        var trimmedNameRu = CategoryRules.NormalizeAndValidateNameRu(nameRu);

        if (_subCategories.Any(x =>
                !x.IsDeleted &&
                (x.Name == trimmedName || x.NameRu == trimmedNameRu)))
        {
            throw new DomainException(
                "Sub category already exists");
        }
        
        var trimmedDescription  = CategoryRules.NormalizeAndValidateDescription(description);
        
        var upperName = trimmedName.ToUpperInvariant();
        var upperNameRu   = trimmedNameRu.ToUpperInvariant();
        
        var subCategoryId = Guid.NewGuid();
        
        Raise(new SubCategoryCreatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            SubCategoryId: subCategoryId,
            Name: trimmedName,
            IsActive: isActive,
            NormalizedName: upperName,
            NameRu: trimmedNameRu,
            NormalizedNameRu: upperNameRu,
            Description: trimmedDescription));
        
        return subCategoryId;
    }

    public void ChangeSubCategoryName(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);

        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        EnsureSubCategoryNotDeleted(subCategory);
        
        var trimmedName = CategoryRules.NormalizeAndValidateName(name);
        
        if (trimmedName == subCategory.Name) return;

        var normalizedName = trimmedName.ToUpperInvariant();
        
        Raise(new SubCategoryNameChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            SubCategoryId: subCategoryId,
            Name: trimmedName,
            NormalizedName: normalizedName));
    }
    
    public void ChangeSubCategoryNameRu(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        EnsureSubCategoryNotDeleted(subCategory);
        
        var trimmedNameRu = CategoryRules.NormalizeAndValidateNameRu(nameRu);
        
        if (trimmedNameRu == subCategory.NameRu) return;
        
        var normalizedNameRu = trimmedNameRu.ToUpperInvariant();
        
        Raise(new SubCategoryNameRuChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            SubCategoryId: subCategoryId,
            NameRu: trimmedNameRu,
            NormalizedNameRu: normalizedNameRu));
    }
    
    public void ChangeSubCategoryDescription( 
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        EnsureSubCategoryNotDeleted(subCategory);
        
        var trimmedDescription = CategoryRules.NormalizeAndValidateDescription(description);
        
        if (trimmedDescription == subCategory.Description) return;
        
        Raise(new SubCategoryDescriptionChangedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            CategoryId: Id,
            SubCategoryId: subCategoryId,
            Description: trimmedDescription));
    }

    public void ActivateSubCategory(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        EnsureSubCategoryNotDeleted(subCategory);

        if (subCategory.IsActive) return;
        
        Raise(new SubCategoryActivatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            SubCategoryId: subCategoryId,
            CategoryId: Id));
    }
    
    public void DeactivateSubCategory(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        EnsureSubCategoryNotDeleted(subCategory);
        
        if (!subCategory.IsActive) return;
        
        Raise(new SubCategoryDeactivatedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            SubCategoryId: subCategoryId,
            CategoryId: Id));
    }
    
    public void DeleteSubCategory(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        
        if (subCategory.IsDeleted) return;
        
        Raise(new SubCategoryDeletedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            SubCategoryId: subCategoryId,
            CategoryId: Id));
    }
    
    public void RestoreSubCategory(
        Guid updatedById,
        string updatedByRole,
        Guid subCategoryId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        CategoryRules.UpdatedByValidation(updatedById, updatedByRole);
        
        var subCategory = GetSubCategory(subCategoryId: subCategoryId);
        
        if (!subCategory.IsDeleted) return;
        
        Raise(new SubCategoryRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            SubCategoryId: subCategoryId,
            CategoryId: Id));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case CategoryCreatedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Id = e.CategoryId;
                Name = e.Name;
                NormalizedName = e.NormalizedName;
                NameRu = e.NameRu;
                NormalizedNameRu = e.NormalizedNameRu;
                Description = e.Description;
                IsActive = e.IsActive;
                IsDeleted = false;
                CreatedAt = e.OccurredAt;
                break;
            
            case CategoryNameChangedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                Name = e.Name;
                NormalizedName = e.NormalizedName;
                break;
            
            case CategoryNameRuChangedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                NameRu = e.NameRu;
                NormalizedNameRu = e.NormalizedNameRu;
                break;
            
            case CategoryDescriptionChangedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                Description = e.Description;
                break;
            
            case CategoryActivatedEvent e:
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                UpdatedById = e.UpdatedById;
                IsActive = true;
                break;
            
            case CategoryDeactivatedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                IsActive = false;
                break;
            
            case CategoryDeletedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                DeletedAt = e.OccurredAt;
                IsActive = false;
                IsDeleted = true;
                break;
            
            case CategoryRestoredEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                DeletedAt = null;
                IsActive = false;
                IsDeleted = false;
                break;
            
            case SubCategoryCreatedEvent e:
                _subCategories.Add(SubCategory.Create(
                    now: e.OccurredAt,
                    categoryId: e.CategoryId,
                    subCategoryId: e.SubCategoryId,
                    updatedById: e.UpdatedById,
                    updatedByRole: e.UpdatedByRole,
                    name: e.Name,
                    normalizedName: e.NormalizedName,
                    nameRu: e.NameRu,
                    normalizedNameRu: e.NormalizedNameRu,
                    description: e.Description,
                    isActive: e.IsActive));
                
                UpdatedAt = e.OccurredAt;
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                break;
            
            case SubCategoryNameChangedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .ChangeName(
                        name: e.Name,
                        normalizedName: e.NormalizedName,
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryNameRuChangedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .ChangeNameRu(
                        nameRu: e.NameRu,
                        normalizedNameRu: e.NormalizedNameRu,
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryDescriptionChangedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .ChangeDescription(
                        description: e.Description,
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryActivatedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .Activate(
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryDeactivatedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .Deactivate(
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryDeletedEvent e:
                GetSubCategory(e.SubCategoryId)
                    .Delete(
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SubCategoryRestoredEvent e:
                GetSubCategory(e.SubCategoryId)
                    .Restore(
                        updatedById: e.UpdatedById,
                        updatedByRole: e.UpdatedByRole,
                        now: e.OccurredAt);
                    
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    public static Category Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var category = new Category();
        
        foreach (var @event in history)
        {
            category.Apply(@event);
            category.Version++;
        }

        return category;
    }
    
    private void EnsureNotDeleted(
        string? message = null)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                message ?? "Category is deleted.");
        }
    }
    
    private void EnsureSubCategoryNotDeleted(
        SubCategory subCategory, 
        string? message = null)
    {
        if (subCategory.IsDeleted)
        {
            throw new DomainException(
                message ?? "Sub Category is deleted.");
        }
    }

    private SubCategory GetSubCategory(Guid subCategoryId) =>
        _subCategories.SingleOrDefault(x => x.Id == subCategoryId)
        ?? throw new DomainException("Sub category is not found");
}