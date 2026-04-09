namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class StockSalesChangedConsumer
    : IConsumer<StockSalesChangedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public StockSalesChangedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<StockSalesChangedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}