using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryNameRuChangedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryNameRuChangedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;

    public CategoryNameRuChangedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryNameRuChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.ChangeNameRuAsync(
            categoryId: notification.DomainEvent.CategoryId,
            nameRu: notification.DomainEvent.NameRu,
            normalizedNameRu: notification.DomainEvent.NormalizedNameRu,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}