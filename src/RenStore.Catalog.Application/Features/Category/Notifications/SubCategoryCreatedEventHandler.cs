using RenStore.Catalog.Domain.Aggregates.Category.Events;

namespace RenStore.Catalog.Application.Features.Category.Notifications;

internal sealed class SubCategoryCreatedEventHandler
    : INotificationHandler<DomainEventNotification<SubCategoryCreatedEvent>>
{
    private readonly ISubCategoryProjection _subCategoryProjection;

    public SubCategoryCreatedEventHandler(
        ISubCategoryProjection subCategoryProjection)
    {
        _subCategoryProjection = subCategoryProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<SubCategoryCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _subCategoryProjection.AddAsync(
            subCategory: new SubCategoryReadModel()
            {
                Id = notification.DomainEvent.SubCategoryId,
                CategoryId = notification.DomainEvent.CategoryId,
                UpdatedByRole = notification.DomainEvent.UpdatedByRole,
                UpdatedById = notification.DomainEvent.UpdatedById,
                CreatedAt = notification.DomainEvent.OccurredAt,
                IsActive = notification.DomainEvent.IsActive,
                Name = notification.DomainEvent.Name,
                NormalizedName = notification.DomainEvent.NormalizedName,
                NameRu = notification.DomainEvent.NameRu,
                NormalizedNameRu = notification.DomainEvent.NormalizedNameRu,
                Description = notification.DomainEvent.Description
            },
            cancellationToken: cancellationToken);
    }
}