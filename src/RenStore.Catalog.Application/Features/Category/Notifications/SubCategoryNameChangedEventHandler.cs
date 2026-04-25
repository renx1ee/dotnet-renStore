using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryNameChangedEventHandler
    : INotificationHandler<DomainEventNotification<SubCategoryNameChangedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryNameChangedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<SubCategoryNameChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.ChangeNameAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            name: notification.DomainEvent.Name,
            normalizedName: notification.DomainEvent.NormalizedName,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}