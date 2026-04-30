using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using RenStore.Order.Persistence.Saga.Configurations;

namespace RenStore.Order.Persistence.Saga;

public sealed class OrderSagaDbContext(DbContextOptions<OrderSagaDbContext> options) 
    : SagaDbContext(options)
{
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new PlaceOrderSagaStateMap(); }
    }
}