using RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeDiscount;

namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class DiscountAvailabilityChangedConsumer(IMediator mediator)
    : IConsumer<DiscountAvailabilityChangedIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<DiscountAvailabilityChangedIntegrationEvent> context)
    {
        var message = context.Message;
        
        await _mediator.Send(new ChangeDiscountProjectionCommand(
            VariantId: message.VariantId,
            OccurredAt: message.OccurredAt,
            DiscountPercents: message.Count));
    }
}