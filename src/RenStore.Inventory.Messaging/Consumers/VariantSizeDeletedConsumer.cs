/*using MassTransit;
using MediatR;
using RenStore.Catalog.Contracts.Events;

namespace RenStore.Inventory.Messaging.Consumers;

internal sealed class VariantSizeDeletedConsumer
    : IConsumer<VariantSizeDeletedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public VariantSizeDeletedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(
        ConsumeContext<VariantSizeDeletedIntegrationEvent> context)
    {
        var message = context.Message;
        
        /*await _mediator.Send(new StockSoftDeleteCommand(
            VariantId: message.VariantId,
            SizeId: message.SizeId,
            InitialStock: 0));#1#
    }
}*/