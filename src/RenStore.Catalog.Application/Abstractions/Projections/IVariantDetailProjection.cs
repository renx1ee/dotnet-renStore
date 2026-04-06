namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantDetailProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantDetailReadModel detail,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantDetailReadModel> detail,
        CancellationToken cancellationToken);

    Task DetailsDescriptionUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string description,
        CancellationToken cancellationToken);

    Task DetailsCompositionUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string composition,
        CancellationToken cancellationToken);
    
    Task DetailsModelFeaturesUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string modelFeatures,
        CancellationToken cancellationToken);

    Task DetailsDecorativeElementsUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string decorativeElements,
        CancellationToken cancellationToken);

    Task DetailsEquipmentUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string equipment,
        CancellationToken cancellationToken);

    Task DetailsCaringOfThingsUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string caringOfThings,
        CancellationToken cancellationToken);

    Task DetailsTypeOfPackingUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        TypeOfPacking typeOfPacking,
        CancellationToken cancellationToken);

    void Remove(VariantDetailReadModel detail);

    void RemoveRange(IReadOnlyCollection<VariantDetailReadModel> details);
}