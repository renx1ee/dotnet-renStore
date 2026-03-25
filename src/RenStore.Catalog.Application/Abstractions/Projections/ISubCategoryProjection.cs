namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface ISubCategoryProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        SubCategoryReadModel subCategory,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<SubCategoryReadModel> subCategories,
        CancellationToken cancellationToken);

    Task ChangeNameAsync(
        DateTimeOffset now,
        string name,
        string normalizedName,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task ChangeNameRuAsync(
        DateTimeOffset now,
        string nameRu,
        string normalizedNameRu,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task ChangeDescriptionAsync(
        DateTimeOffset now,
        string description,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task ActivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task DeactivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        DateTimeOffset now,
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);
    
    Task<bool> IsExistsAsync(
        Guid categoryId,
        string name,
        string nameRu,
        CancellationToken cancellationToken);
    
    Task<bool> IsExistsAsync(
        string name,
        string nameRu,
        CancellationToken cancellationToken);

    Task<bool> IsExistsAsync(
        Guid categoryId,
        Guid subCategoryId,
        CancellationToken cancellationToken);

    void Remove(SubCategoryReadModel subCategory);

    void RemoveRange(IReadOnlyCollection<SubCategoryReadModel> subCategories);
}