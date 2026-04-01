using MassTransit;
using MediatR; 
using RenStore.Catalog.Contracts.Events;
using RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

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
        
        await _mediator.Send(new StockSoftDeleteCommand(
            VariantId: message.VariantId,
            SizeId: message.SizeId));
    }
}