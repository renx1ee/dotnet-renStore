using Microsoft.EntityFrameworkCore;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class VariantDetailProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantDetailProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantDetailProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        VariantDetailReadModel detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        await _context.Details.AddAsync(detail, cancellationToken);

        return detail.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantDetailReadModel> detail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(detail);

        var detailsList = detail as IList<VariantDetailReadModel> ?? detail.ToList();
        
        if (detailsList.Count == 0) return;

        await _context.Details.AddRangeAsync(detailsList, cancellationToken);
    }

    public async Task DetailsDescriptionUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string description,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(description);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.Description = description;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsCompositionUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string composition,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(composition);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.Composition = composition;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsEquipmentUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string equipment,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(equipment);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.Equipment = equipment;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsModelFeaturesUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string modelFeatures,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(modelFeatures);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.ModelFeatures = modelFeatures;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsDecorativeElementsUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string decorativeElements,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(decorativeElements);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.DecorativeElements = decorativeElements;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsCaringOfThingsUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        string caringOfThings,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);
        ValidateStringParameters(caringOfThings);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.CaringOfThings = caringOfThings;
        details.UpdatedAt = now;
    }
    
    public async Task DetailsTypeOfPackingUpdateAsync(
        Guid detailsId,
        DateTimeOffset now,
        TypeOfPacking typeOfPacking,
        CancellationToken cancellationToken)
    {
        ValidateDetailsId(detailsId);

        var details = await GetDetailsAsync(
            detailsId: detailsId,
            cancellationToken: cancellationToken);

        details.TypeOfPacking = typeOfPacking;
        details.UpdatedAt = now;
    }

    public void Remove(VariantDetailReadModel detail)
    {
        ArgumentNullException.ThrowIfNull(detail);

        _context.Details.Remove(detail);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantDetailReadModel> details)
    {
        ArgumentNullException.ThrowIfNull(details);

        _context.Details.RemoveRange(details);
    }
    
    private async Task<VariantDetailReadModel> GetDetailsAsync(
        Guid detailsId,
        CancellationToken cancellationToken)
    {
        var view = await _context.Details
            .FirstOrDefaultAsync(x =>
                    x.Id == detailsId,
                cancellationToken: cancellationToken);
        
        if (view is null)
        {
            throw new NotFoundException(
                name: typeof(VariantDetailReadModel),
                detailsId);
        }

        return view;
    }

    private static void ValidateStringParameters(string param)
    {
        if (string.IsNullOrWhiteSpace(param))
            throw new ArgumentException(nameof(param));
    }
    
    private static void ValidateDetailsId(Guid detailsId)
    {
        if (detailsId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(detailsId));
    }
}