using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public class DeliveryOrderTests
{
    #region Create
    [Fact]
    public async Task CreateDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
    }
    
    [Fact]
    public async Task CreateDeliveryOrder_FailOnWrongOrderId_Test()
    {
        // Arrange
        Guid orderId = Guid.Empty;
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;

        // Act & Assert
        Assert.Throws<DomainException>(() => 
            DeliveryOrder.Create(
                orderId: orderId,
                deliveryTariffId: deliveryTariffId, 
                now: now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateDeliveryOrder_FailOnWrongDeliveryTariffId_Test(int deliveryTariffId)
    {
        // Arrange
        Guid orderId = Guid.Empty;
        DateTimeOffset now = DateTimeOffset.UtcNow;

        // Act & Assert
        Assert.Throws<DomainException>(() => 
            DeliveryOrder.Create(
                orderId: orderId,
                deliveryTariffId: deliveryTariffId, 
                now: now));
    }
    #endregion
    #region MarkAsAssemblingBySeller
    [Fact]
    public async Task MarkAsAssemblingBySellerDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);

        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        
        // Assert
        Assert.Equal(DeliveryStatus.AssemblingBySeller, result.Status);
    }
    
    [Fact]
    public async Task MarkAsAssemblingBySellerDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);

        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsAssemblingBySeller(now));
    }
    
    [Fact]
    public async Task MarkAsAssemblingBySellerDeliveryOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);

        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsAssemblingBySeller(now));
    }
    #endregion
    #region ShipToSortingCenter 
    [Fact]
    public async Task ShipToSortingCenterDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);

        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        
        // Assert
        Assert.Equal(DeliveryStatus.EnRouteToSortingCenter, result.Status);
    }
    
    [Fact]
    public async Task ShipToSortingCenterDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToSortingCenter(1, now));
    }
    
    [Fact]
    public async Task ShipToSortingCenterOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToSortingCenter(1, now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ShipToSortingCenterDeliveryOrder_FailOnWrongSortingCenterId_Test(
        int sortingCenterId)
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);

        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToSortingCenter(sortingCenterId, now));
    }
    #endregion
    #region MarkAsArrivedAtSortingCenter
    [Fact]
    public async Task MarkAsArrivedAtSortingCenterDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        
        // Assert
        Assert.Equal(DeliveryStatus.ArrivedAtSortingCenter, result.Status);
    }
    
    [Fact]
    public async Task MarkAsArrivedAtSortingCenterDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsArrivedAtSortingCenter(1, now));
    }
    
    [Fact]
    public async Task MarkAsArrivedAtSortingCenterOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsArrivedAtSortingCenter(1, now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public async Task MarkAsArrivedAtSortingCenterOrder_FailOnWrongSortingCenterId_Test(
        int sortingCenterId)
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsArrivedAtSortingCenter(sortingCenterId, now));
    }
    #endregion
    #region SortAtSortingCenter
    [Fact]
    public async Task SortAtSortingCenterDeliveryDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        
        // Assert
        Assert.Equal(DeliveryStatus.Sorted, result.Status);
    }
    
    [Fact]
    public async Task SortAtSortingCenterDeliveryDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.SortAtSortingCenter(1, now));
    }
    
    [Fact]
    public async Task SortAtSortingCenterDeliveryOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.SortAtSortingCenter(1, now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public async Task SortAtSortingCenterDeliveryOrder_FailOnWrongSortingCenterId_Test(
        int sortingCenterId)
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.SortAtSortingCenter(sortingCenterId, now));
    }
    #endregion
    #region ShipToPickupPoint
    [Fact]
    public async Task ShipToPickupPointDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        
        // Assert
        Assert.Equal(DeliveryStatus.EnRouteToPickupPoint, result.Status);
    }
    
    [Fact]
    public async Task ShipToPickupPointDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToPickupPoint(1, now));
    }
    
    [Fact]
    public async Task ShipToPickupPointDeliveryOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToPickupPoint(1, now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ShipToPickupPointDeliveryOrder_FailOnWrongPickupPointId_Test(
        int pickupPointId)
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ShipToPickupPoint(pickupPointId, now));
    }
    #endregion
    #region MarkAsAwaitingPickup
    [Fact]
    public async Task MarkAsAwaitingPickupDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        result.MarkAsAwaitingPickup(1, now);
        
        // Assert
        Assert.Equal(DeliveryStatus.AwaitingPickup, result.Status);
    }
    
    [Fact]
    public async Task MarkAsAwaitingPickupDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsAwaitingPickup(1, now));
    }
    
    [Fact]
    public async Task MarkAsAwaitingPickupDeliveryOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsAwaitingPickup(1, now));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    public async Task MarkAsAwaitingPickupDeliveryOrder_FailOnWrongPickupPointId_Test(
        int pickupPointId)
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsAwaitingPickup(pickupPointId, now));
    }
    #endregion
    #region MarkAsDelivered
    [Fact]
    public async Task MarkAsDeliveredDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        result.MarkAsAwaitingPickup(1, now);
        result.MarkAsDelivered(now);
        
        // Assert
        Assert.Equal(DeliveryStatus.Delivered, result.Status);
    }
    
    [Fact]
    public async Task MarkAsDeliveredDeliveryOrder_FailOnWrongStatus_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsDelivered(now));
    }
    
    [Fact]
    public async Task MarkAsDeliveredDeliveryOrder_FailOnDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);
        
        result.MarkAsAssemblingBySeller(now);
        result.ShipToSortingCenter(1, now);
        result.MarkAsArrivedAtSortingCenter(1, now);
        result.SortAtSortingCenter(1, now);
        result.ShipToPickupPoint(1, now);
        result.MarkAsAwaitingPickup(1, now);
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.MarkAsDelivered(now));
    }
    #endregion
    #region Delete
    [Fact]
    public async Task DeleteDeliveryOrder_Success_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);

        result.Delete(now);
        
        // Assert
        Assert.Equal(DeliveryStatus.IsDeleted, result.Status);
        Assert.Equal(now, result.DeletedAt);
    }
    
    [Fact]
    public async Task DeleteDeliveryOrder_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        int deliveryTariffId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryOrder.Create(
            orderId: orderId, 
            deliveryTariffId: deliveryTariffId, 
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(deliveryTariffId, result.DeliveryTariffId);
        Assert.Equal(now, result.CreatedAt);

        result.Delete(now);
        
        // Assert
        Assert.Equal(DeliveryStatus.IsDeleted, result.Status);
        Assert.Equal(now, result.DeletedAt);

        Assert.Throws<DomainException>(() => result.Delete(now));
    }

    #endregion
    // TODO: Return
}