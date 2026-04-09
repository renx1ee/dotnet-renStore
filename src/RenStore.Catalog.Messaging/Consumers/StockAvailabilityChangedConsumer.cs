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
        throw new NotImplementedException();
    }
}