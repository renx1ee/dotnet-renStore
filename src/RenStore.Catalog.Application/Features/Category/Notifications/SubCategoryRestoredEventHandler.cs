using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryRestoredEventHandler
    : INotificationHandler<DomainEventNotification<SubCategoryRestoredEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryRestoredEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
        
    public async Task Handle(
        DomainEventNotification<SubCategoryRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.RestoreAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _subCategoryProjection.SaveChangesAsync(cancellationToken);
    }
}