using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryNameChangedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryNameChangedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;

    public CategoryNameChangedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryNameChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.ChangeNameAsync(
            categoryId: notification.DomainEvent.CategoryId,
            name: notification.DomainEvent.Name,
            normalizedName: notification.DomainEvent.NormalizedName,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _categoryProjection.CommitAsync(cancellationToken);
    }
}