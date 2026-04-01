using MassTransit;
using MediatR;
using RenStore.Catalog.Contracts.Events;
using RenStore.Inventory.Application.Features.Stock.Commands.Create;

namespace RenStore.Inventory.Messaging.Consumers;

internal sealed class VariantSizeCreatedConsumer
    : IConsumer<VariantSizeCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;
    
    public VariantSizeCreatedConsumer(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<VariantSizeCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        
        await _mediator.Send(new CreateStockCommand(
            VariantId: message.VariantId,
            SizeId: message.SizeId,
            InitialStock: 0));
    }
}