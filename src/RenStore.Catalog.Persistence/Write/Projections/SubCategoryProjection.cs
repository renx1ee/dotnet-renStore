using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class SubCategoryProjection 
    : RenStore.Catalog.Application.Abstractions.Projections.ISubCategoryProjection
{
    private readonly CatalogDbContext _context;
    
    public SubCategoryProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        SubCategoryReadModel subCategory,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subCategory);
        
        await _context.SubCategories.AddAsync(subCategory, cancellationToken);
        
        return subCategory.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<SubCategoryReadModel> subCategories,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subCategories);

        var subCategoriesList = subCategories as IList<SubCategoryReadModel> ?? subCategories.ToList();

        if (subCategoriesList.Count == 0) return;

        await _context.SubCategories.AddRangeAsync(subCategoriesList, cancellationToken);
    }
    
    public async Task ChangeNameAsync(
        DateTimeOffset now,
        string name,
        string normalizedName,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.Name = name;
        subCategory.NormalizedName = normalizedName;
        subCategory.IsActive = true;
        subCategory.UpdatedAt = now;
    }
    
    public async Task ChangeNameRuAsync(
        DateTimeOffset now,
        string nameRu,
        string normalizedNameRu,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.NameRu = nameRu;
        subCategory.NormalizedNameRu = normalizedNameRu;
        subCategory.IsActive = true;
        subCategory.UpdatedAt = now;
    }
    
    public async Task ChangeDescriptionAsync(
        DateTimeOffset now,
        string description,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);
        
        subCategory.Description = description;
        subCategory.IsActive = true;
        subCategory.UpdatedAt = now;
    }
    
    public async Task ActivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.IsActive = true;
        subCategory.UpdatedAt = now;
    }
    
    public async Task DeactivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.IsActive = false;
        subCategory.UpdatedAt = now;
    }
    
    public async Task SoftDeleteAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.IsActive = false;
        subCategory.IsDeleted = true;
        subCategory.DeletedAt = now;
        subCategory.UpdatedAt = now;
    }
    
    public async Task RestoreAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var subCategory = await GetSubCategory(categoryId, subCategoryId, cancellationToken);

        subCategory.IsActive = false;
        subCategory.IsDeleted = false;
        subCategory.DeletedAt = null;
        subCategory.UpdatedAt = now;
    }
    
    public async Task<bool> IsExistsAsync(
        Guid categoryId,
        string name,
        string nameRu,
        CancellationToken cancellationToken)
    {
        return await _context.SubCategories
            .AnyAsync(x => 
                    x.CategoryId == categoryId &&
                    (x.Name == name || x.NameRu == nameRu), 
                cancellationToken);
    }

    public void Remove(SubCategoryReadModel subCategory)
    {
        ArgumentNullException.ThrowIfNull(subCategory);
        
        _context.SubCategories.Remove(subCategory);
    }
    
    public void RemoveRange(IReadOnlyCollection<SubCategoryReadModel> subCategories)
    {
        ArgumentNullException.ThrowIfNull(subCategories);

        _context.SubCategories.RemoveRange(subCategories);
    }
    
    private async Task<SubCategoryReadModel> GetSubCategory(
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var category = await _context.SubCategories
            .FirstOrDefaultAsync(x =>
                    x.CategoryId == categoryId &&
                    x.Id == subCategoryId,
                cancellationToken);

        if (category is null)
            throw new NotFoundException(
                name: typeof(SubCategoryReadModel),
                subCategoryId);

        return category;
    }
}