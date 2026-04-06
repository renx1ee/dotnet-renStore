using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryDescriptionChangedEventHandler
    : INotificationHandler<DomainEventNotification<SubCategoryDescriptionChangedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryDescriptionChangedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<SubCategoryDescriptionChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.ChangeDescriptionAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            description: notification.DomainEvent.Description,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _subCategoryProjection.CommitAsync(cancellationToken);
    }
}