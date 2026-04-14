using RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeStock;

namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class StockAvailabilityChangedConsumer
    : IConsumer<StockAvailabilityChangedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public StockAvailabilityChangedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<StockAvailabilityChangedIntegrationEvent> context)
    {
        var message = context.Message;
        
        await _mediator.Send(new ChangeChangeStockProjectionCommand(
            VariantId: message.VariantId,
            OccurredAt: message.OccurredAt,
            InStock: message.Count));
    }
}