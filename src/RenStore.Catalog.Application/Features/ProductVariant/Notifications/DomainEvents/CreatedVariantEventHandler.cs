using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class CreatedVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantCreatedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;
    
    public CreatedVariantEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.AddAsync(new ProductVariantReadModel()
        {
            Id = notification.DomainEvent.VariantId,
            ProductId = notification.DomainEvent.ProductId,
            ColorId = notification.DomainEvent.ColorId,
            CreatedAt = notification.DomainEvent.OccurredAt,
            Name = notification.DomainEvent.Name,
            NormalizedName = notification.DomainEvent.NormalizedName,
            SizeSystem = notification.DomainEvent.SizeSystem,
            SizeType = notification.DomainEvent.SizeType,
            Article = notification.DomainEvent.Article,
            Status = notification.DomainEvent.Status,
            Url = notification.DomainEvent.Url
        }, cancellationToken);
    }
}