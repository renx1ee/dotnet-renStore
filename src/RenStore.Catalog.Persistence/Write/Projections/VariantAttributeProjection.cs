using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class VariantAttributeProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantAttributeProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantAttributeProjection(
        CatalogDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(
        VariantAttributeReadModel attribute,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        await _context.Attributes.AddAsync(attribute, cancellationToken);

        return attribute.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantAttributeReadModel> attributes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        var attributesList = attributes as IList<VariantAttributeReadModel> ?? attributes.ToList();
        
        if(attributesList.Count == 0) return;

        await _context.Attributes.AddRangeAsync(attributesList, cancellationToken);
    }
    
    public async Task UpdateKeyAsync(
        Guid attributeId,
        string key,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateAttributeId(attributeId);

        var attribute = await GetAttributeAsync(
            attributeId: attributeId,
            cancellationToken: cancellationToken);

        attribute.Key = AttributeKey.Create(key);
        attribute.UpdatedAt = now;
    }
    
    public async Task UpdateValueAsync(
        Guid attributeId,
        string value,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateAttributeId(attributeId);

        var attribute = await GetAttributeAsync(
            attributeId: attributeId,
            cancellationToken: cancellationToken);

        attribute.Value = AttributeValue.Create(value);
        attribute.UpdatedAt = now;
    }
    
    public async Task SoftDeleteAsync(
        Guid attributeId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateAttributeId(attributeId);

        var attribute = await GetAttributeAsync(
            attributeId: attributeId,
            cancellationToken: cancellationToken);

        attribute.UpdatedAt = now;
        attribute.DeletedAt = now;
        attribute.IsDeleted = true;
    }
    
    public async Task RestoreAsync(
        Guid attributeId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateAttributeId(attributeId);

        var attribute = await GetAttributeAsync(
            attributeId: attributeId,
            cancellationToken: cancellationToken);

        attribute.UpdatedAt = now;
        attribute.DeletedAt = null;
        attribute.IsDeleted = false;
    }
    
    public void Remove(VariantAttributeReadModel attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);

        _context.Attributes.Remove(attribute);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantAttributeReadModel> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        _context.Attributes.RemoveRange(attributes);
    }
    
    private async Task<VariantAttributeReadModel> GetAttributeAsync(
        Guid attributeId,
        CancellationToken cancellationToken)
    {
        var view = await _context.Attributes
            .FirstOrDefaultAsync(x =>
                    x.Id == attributeId,
                cancellationToken: cancellationToken);
        
        if (view is null)
        {
            throw new NotFoundException(
                name: typeof(VariantAttributeReadModel),
                attributeId);
        }

        return view;
    }
    
    private static void ValidateAttributeId(Guid attributeId)
    {
        if (attributeId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(attributeId));
    }
}