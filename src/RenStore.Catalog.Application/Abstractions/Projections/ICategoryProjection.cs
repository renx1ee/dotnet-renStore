namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface ICategoryProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        CategoryReadModel category,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<CategoryReadModel> categories,
        CancellationToken cancellationToken);

    Task<bool> IsExistsAsync(
        string name,
        string nameRu,
        CancellationToken cancellationToken);

    Task ChangeNameAsync(
        DateTimeOffset now,
        string name,
        string normalizedName,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task ChangeNameRuAsync(
        DateTimeOffset now,
        string nameRu,
        string normalizedNameRu,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task ChangeDescriptionAsync(
        DateTimeOffset now,
        string description,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task ActivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task DeactivateAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        DateTimeOffset now,
        Guid categoryId,
        CancellationToken cancellationToken);

    void Remove(CategoryReadModel category);

    void RemoveRange(IReadOnlyCollection<CategoryReadModel> categories);
}