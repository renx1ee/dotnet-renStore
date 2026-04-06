using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryDeletedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryDeletedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryDeletedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryDeletedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.SoftDeleteAsync(
            categoryId: notification.DomainEvent.CategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _categoryProjection.CommitAsync(cancellationToken);
    }
}