namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class ReviewsCountChangedConsumer
    : IConsumer<ReviewsCountChangedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public ReviewsCountChangedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<ReviewsCountChangedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}