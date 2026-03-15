using MediatR;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Media.Events;

namespace RenStore.Catalog.Application.Features.VariantMedia.Notifications.UnmarkAsMain;

internal sealed class VariantImageUnsetAsMainEventHandler
    : INotificationHandler<DomainEventNotification<ImageMainUnsetEvent>>
{
    private readonly IVariantImageProjection _variantImageProjection;

    public VariantImageUnsetAsMainEventHandler(
        IVariantImageProjection variantImageProjection)
    {
        _variantImageProjection = variantImageProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ImageMainUnsetEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantImageProjection.UnmarkAsMain(
            now: notification.DomainEvent.OccurredAt,
            variantId: notification.DomainEvent.VariantId,
            cancellationToken: cancellationToken);

        await _variantImageProjection.SaveChangesAsync(cancellationToken);
    }
}