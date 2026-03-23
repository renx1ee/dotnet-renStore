namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantAttributeProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantAttributeReadModel attribute,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantAttributeReadModel> attributes,
        CancellationToken cancellationToken);

    Task UpdateKeyAsync(
        Guid attributeId,
        string key,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task UpdateValueAsync(
        Guid attributeId,
        string value,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        Guid attributeId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        Guid attributeId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    void Remove(VariantAttributeReadModel attribute);

    void RemoveRange(IReadOnlyCollection<VariantAttributeReadModel> attributes);
}