using RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSellerVerify;

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
        var message = context.Message;
        
        await _mediator.Send(
            new ChangeSellerVerificationProjectionCommand(
                VariantId: message.VariantId,
                OccurredAt: message.OccurredAt,
                IsVerified: message.IsVarified),
            context.CancellationToken);
    }
}