using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class CategoryCreatedEventHandler
    : INotificationHandler<DomainEventNotification<CategoryCreatedEvent>>
{
    private readonly ICategoryProjection _categoryProjection;
    
    public CategoryCreatedEventHandler(
        ICategoryProjection categoryProjection)
    {
        _categoryProjection = categoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<CategoryCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _categoryProjection.AddAsync(
            category: new CategoryReadModel()
            {
                Id = notification.DomainEvent.CategoryId,
                UpdatedById = notification.DomainEvent.UpdatedById,
                UpdatedByRole = notification.DomainEvent.UpdatedByRole,
                CreatedAt = notification.DomainEvent.OccurredAt,
                IsActive = notification.DomainEvent.IsActive,
                IsDeleted = false,
                Name = notification.DomainEvent.Name,
                NormalizedName = notification.DomainEvent.NormalizedName,
                NameRu = notification.DomainEvent.NameRu,
                NormalizedNameRu = notification.DomainEvent.NormalizedNameRu,
                Description = notification.DomainEvent.Description
            }, 
            cancellationToken: cancellationToken);

        await _categoryProjection.SaveChangesAsync(cancellationToken);
    }
}