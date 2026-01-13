using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public class DeliveryTrackingTests
{
    [Fact]
    public async Task CreateDeliveryTracking_Success_Test()
    {
        // Arrange
        string currentLocation = "fwefew";
        string notes = "fwefwa";
        var now = DateTimeOffset.UtcNow;
        var deliveryOrderId = Guid.NewGuid();
        var sortingCenterId = 1;
        
        // Act
        var result = DeliveryTracking.Create(
            currentLocation: currentLocation,
            status: DeliveryStatus.Delayed,
            occurredAt: now,
            notes: notes,
            deliveryOrderId: deliveryOrderId,
            sortingCenterId: sortingCenterId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(currentLocation, result.CurrentLocation);
        Assert.Equal(notes, result.Notes);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(deliveryOrderId, result.DeliveryOrderId);
        Assert.Equal(sortingCenterId, result.SortingCenterId);
        Assert.Equal(DeliveryStatus.Delayed, result.Status);
    }
    
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public async Task CreateDeliveryTracking_FailOnWrongIds_Test(
        int sortingCenterId,
        int pickupPointId)
    {
        // Arrange
        string currentLocation = "fweffewew";
        string notes = "fwefwa";
        var now = DateTimeOffset.UtcNow;
        var deliveryOrderId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => 
            DeliveryTracking.Create(
                currentLocation: currentLocation,
                status: DeliveryStatus.Delayed,
                occurredAt: now,
                notes: notes,
                deliveryOrderId: deliveryOrderId,
                sortingCenterId: sortingCenterId,
                pickupPointId: pickupPointId));
    }

    [Fact]
    public async Task DeleteDeliveryTracking_Success_Test()
    {
        // Arrange
        string currentLocation = "fwefew";
        string notes = "fwefwa";
        var now = DateTimeOffset.UtcNow;
        var deliveryOrderId = Guid.NewGuid();
        var sortingCenterId = 1;
        
        // Act
        var result = DeliveryTracking.Create(
            currentLocation: currentLocation,
            status: DeliveryStatus.Delayed,
            occurredAt: now,
            notes: notes,
            deliveryOrderId: deliveryOrderId,
            sortingCenterId: sortingCenterId);
        
        result.Delete(now);

        // Assert
        Assert.True(result.IsDeleted);
        Assert.Equal(result.DeletedAt, now);
    }
    
    [Fact]
    public async Task DeleteDeliveryTracking_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string currentLocation = "fwefew";
        string notes = "fwefwa";
        var now = DateTimeOffset.UtcNow;
        var deliveryOrderId = Guid.NewGuid();
        var sortingCenterId = 1;
        
        // Act
        var result = DeliveryTracking.Create(
            currentLocation: currentLocation,
            status: DeliveryStatus.Delayed,
            occurredAt: now,
            notes: notes,
            deliveryOrderId: deliveryOrderId,
            sortingCenterId: sortingCenterId);
        
        result.Delete(now);

        // Assert
        Assert.Throws<DomainException>(() => result.Delete(now));
    }
}