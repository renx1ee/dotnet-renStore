using MediatR;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Features.VariantMedia.Notifications.ImageUploaded;

internal sealed class VariantImageUploadedEventHandler
    : INotificationHandler<DomainEventNotification<ImageCreatedEvent>>
{
    private readonly IVariantImageProjection _variantImageProjection;

    public VariantImageUploadedEventHandler(
        IVariantImageProjection variantImageProjection)
    {
        _variantImageProjection = variantImageProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ImageCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var image = new VariantImageReadModel()
        {
            Id = notification.DomainEvent.ImageId,
            OriginalFileName = notification.DomainEvent.OriginalFileName,
            StoragePath = notification.DomainEvent.StoragePath,
            FileSizeBytes = notification.DomainEvent.FileSizeBytes,
            IsMain = notification.DomainEvent.IsMain,
            SortOrder = notification.DomainEvent.SortOrder,
            Weight = notification.DomainEvent.Weight,
            Height = notification.DomainEvent.Height,
            IsDeleted = false,
            UpdatedAt = notification.DomainEvent.OccurredAt,
            VariantId = notification.DomainEvent.VariantId
        };

        await _variantImageProjection.AddAsync(
            image: image, cancellationToken: cancellationToken);

        await _variantImageProjection.SaveChangesAsync(cancellationToken);
    }
}