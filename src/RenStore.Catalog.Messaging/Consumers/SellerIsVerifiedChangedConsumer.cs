namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class SellerIsVerifiedChangedConsumer
    : IConsumer<SellerIsVerifiedChangedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public SellerIsVerifiedChangedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SellerIsVerifiedChangedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}