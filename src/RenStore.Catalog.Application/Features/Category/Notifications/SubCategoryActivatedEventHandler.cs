using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryActivatedEventHandler 
    : INotificationHandler<DomainEventNotification<SubCategoryActivatedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;

    public SubCategoryActivatedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<SubCategoryActivatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.ActivateAsync(
            now: notification.DomainEvent.OccurredAt,
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            cancellationToken: cancellationToken);
    }
}