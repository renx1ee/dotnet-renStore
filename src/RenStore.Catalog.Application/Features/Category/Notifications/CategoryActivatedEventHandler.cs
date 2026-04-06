using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryActivatedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryActivatedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryActivatedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryActivatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.ActivateAsync(
            now: notification.DomainEvent.OccurredAt,
            categoryId: notification.DomainEvent.CategoryId,
            cancellationToken: cancellationToken);

        await _categoryProjection.CommitAsync(cancellationToken);
    }
}