using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryDeletedEventHandler 
    : INotificationHandler<DomainEventNotification<SubCategoryDeletedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryDeletedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
        
    public async Task Handle(
        DomainEventNotification<SubCategoryDeletedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.SoftDeleteAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}