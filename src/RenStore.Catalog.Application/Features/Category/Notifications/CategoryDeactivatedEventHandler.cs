using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryDeactivatedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryDeactivatedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryDeactivatedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryDeactivatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.DeactivateAsync(
            categoryId: notification.DomainEvent.CategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}