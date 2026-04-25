using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal class CategoryRestoredEventHandler
    : INotificationHandler<DomainEventNotification<CategoryRestoredEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryRestoredEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.RestoreAsync(
            categoryId: notification.DomainEvent.CategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}