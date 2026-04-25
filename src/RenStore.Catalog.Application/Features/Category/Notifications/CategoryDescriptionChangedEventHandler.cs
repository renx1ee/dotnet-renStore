using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryDescriptionChangedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryDescriptionChangedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryDescriptionChangedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryDescriptionChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.ChangeDescriptionAsync(
            categoryId: notification.DomainEvent.CategoryId,
            description: notification.DomainEvent.Description,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}