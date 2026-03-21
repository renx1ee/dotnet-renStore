namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.IntegrationEvents;

internal sealed class VariantDeletedIntegrationEventHandler
    : INotificationHandler<ProductDeletedIntegrationEvent>
{
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductVariantRepository _variantRepository;

    public VariantDeletedIntegrationEventHandler(
        IProductVariantQuery variantQuery,
        IProductVariantRepository variantRepository)
    {
        _variantQuery = variantQuery;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        ProductDeletedIntegrationEvent notification, 
        CancellationToken cancellationToken)
    {
        var variants = await _variantQuery.FindByProductIdAsync(
            notification.ProductId,
            cancellationToken: cancellationToken);

        if (variants is null or []) return;

        var resultVariants = new List<Domain.Aggregates.Variant.ProductVariant>();

        foreach (var variant in variants)
        {
            var rehydratedVariant = await _variantRepository
                .GetAsync(variant.Id, cancellationToken);
            
            if(rehydratedVariant is null)
                throw new NotFoundException(
                    name: typeof(Domain.Aggregates.Variant.ProductVariant),
                    notification.ProductId);
            
            rehydratedVariant.Delete(
                now: notification.OccurredAt,
                updatedById: notification.UpdatedById,
                updatedByRole: notification.UpdatedByRole);
            
            resultVariants.Add(rehydratedVariant);
        }
        
        await _variantRepository.SaveManyAsync(resultVariants, cancellationToken);
    }
}