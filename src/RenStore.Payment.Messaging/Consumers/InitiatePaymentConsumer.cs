using MassTransit;
using RenStore.Payment.Contracts.IntegrationEvents;

namespace RenStore.Payment.Messaging.Consumers;

internal sealed class InitiatePaymentConsumer
    : IConsumer<InitiatePaymentIntegrationEvent>
{
    public Task Consume(ConsumeContext<InitiatePaymentIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}