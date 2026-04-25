using RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeReviewsCount;

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
        var message = context.Message;

        await _mediator.Send(
            new ChangeReviewsCountProjectionCommand(
                VariantId: message.VariantId,
                OccurredAt: message.OccurredAt,
                AverageRating: message.AverageRating,
                Count: message.Count),
            context.CancellationToken);
    }
}