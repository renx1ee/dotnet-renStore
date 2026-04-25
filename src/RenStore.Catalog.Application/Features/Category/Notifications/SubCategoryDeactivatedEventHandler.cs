using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryDeactivatedEventHandler 
    : INotificationHandler<DomainEventNotification<SubCategoryDeactivatedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryDeactivatedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
        
    public async Task Handle(
        DomainEventNotification<SubCategoryDeactivatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.DeactivateAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}