/*using Microsoft.Extensions.Logging;
using RenStore.Application.Repository;
using RenStore.Domain.Enums;

namespace RenStore.Application.Services;

public class OrderService 
{
    private readonly ILogger<OrderService> logger;
    private readonly IOrderRepository orderRepository;

    public OrderService(ILogger<OrderService> logger,
        IOrderRepository orderRepository)
    {
        this.logger = logger;
        this.orderRepository = orderRepository;
    }

    public async Task<int> CreateZipCodeAsync()
    {
        Random random = new Random();
        return random.Next(1000, 9999);
    }

    public async Task<DeliveryStatus> ChangeOrderDeliveryStatus()
    {
        return DeliveryStatus.AwaitingConfirmation;
    }

    public async Task<decimal> GetOrderTotalPrice(decimal price, uint amount)
    {
        return price * amount;
    }
}*/