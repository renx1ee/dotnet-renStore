using RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSales;

namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class StockSalesChangedConsumer
    : IConsumer<StockSelesCountChangedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public StockSalesChangedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<StockSelesCountChangedIntegrationEvent> context)
    {
        var message = context.Message;
        
        await _mediator.Send(new ChangeSalesCountProjectionCommand(
            VariantId: message.VariantId,
            OccurredAt: message.OccurredAt,
            Sales: message.Count));
    }
}