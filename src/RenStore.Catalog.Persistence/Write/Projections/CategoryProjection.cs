using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class CategoryProjection 
    : RenStore.Catalog.Application.Abstractions.Projections.ICategoryProjection
{
    private readonly CatalogDbContext _context;
    
    public CategoryProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task CommitAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        CategoryReadModel category,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);
        
        await _context.Categories.AddAsync(category, cancellationToken);
        
        return category.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<CategoryReadModel> categories,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(categories);

        var categoriesList = categories as IList<CategoryReadModel> ?? categories.ToList();

        if (categoriesList.Count == 0) return;

        await _context.Categories.AddRangeAsync(categoriesList, cancellationToken);
    }

    public async Task ChangeNameAsync(
        DateTimeOffset now,
        string name,
        string normalizedName,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.Name = name;
        category.NormalizedName = normalizedName;
        category.IsActive = true;
        category.UpdatedAt = now;
    }

    public async Task ChangeNameRuAsync(
        DateTimeOffset now,
        string nameRu,
        string normalizedNameRu,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.NameRu = nameRu;
        category.NormalizedNameRu = normalizedNameRu;
        category.IsActive = true;
        category.UpdatedAt = now;
    }
    
    public async Task ChangeDescriptionAsync(
        DateTimeOffset now,
        string description,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);
        
        category.Description = description;
        category.IsActive = true;
        category.UpdatedAt = now;
    }

    public async Task ActivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.IsActive = true;
        category.UpdatedAt = now;
    }
    
    public async Task DeactivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.IsActive = false;
        category.UpdatedAt = now;
    }
    
    public async Task SoftDeleteAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.IsActive = false;
        category.IsDeleted = true;
        category.DeletedAt = now;
        category.UpdatedAt = now;
    }
    
    public async Task RestoreAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await GetCategory(categoryId, cancellationToken);

        category.IsActive = false;
        category.IsDeleted = false;
        category.DeletedAt = null;
        category.UpdatedAt = now;
    }


    public async Task<bool> IsExistsAsync(
        string name,
        string nameRu,
        CancellationToken cancellationToken)
    {
        return _context.Categories.Any(x =>
            x.Name == name ||
            x.NameRu == nameRu);
    }

    public void Remove(CategoryReadModel category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Categories.Remove(category);
    }
    
    public void RemoveRange(IReadOnlyCollection<CategoryReadModel> categories)
    {
        ArgumentNullException.ThrowIfNull(categories);

        _context.Categories.RemoveRange(categories);
    }

    private async Task<CategoryReadModel> GetCategory(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x =>
                x.Id == categoryId,
                cancellationToken);

        if (category is null)
            throw new NotFoundException(
                name: typeof(CategoryReadModel),
                categoryId);

        return category;
    }
}