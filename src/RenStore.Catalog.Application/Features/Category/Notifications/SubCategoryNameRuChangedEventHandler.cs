using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryNameRuChangedEventHandler
    : INotificationHandler<DomainEventNotification<SubCategoryNameRuChangedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;
        
    public SubCategoryNameRuChangedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<SubCategoryNameRuChangedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.ChangeNameRuAsync(
            categoryId: notification.DomainEvent.CategoryId,
            subCategoryId: notification.DomainEvent.SubCategoryId,
            nameRu: notification.DomainEvent.NameRu,
            normalizedNameRu: notification.DomainEvent.NormalizedNameRu,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}