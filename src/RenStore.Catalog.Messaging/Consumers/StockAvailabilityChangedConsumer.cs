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
        
        await _mediator.Send(
            new ChangeChangeStockProjectionCommand(
                OccurredAt: message.OccurredAt,
                VariantId:  message.VariantId,
                SizeId:     message.SizeId,
                InStock:    message.Count,
                Sales:      message.Sales),
            context.CancellationToken);
    }
}