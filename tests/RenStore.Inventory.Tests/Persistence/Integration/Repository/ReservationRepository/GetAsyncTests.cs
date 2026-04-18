/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Inventory.Domain.Aggregates.Reservation;
using RenStore.Inventory.Persistence;
using RenStore.Inventory.Persistence.EventStore;
using Xunit;
using Xunit.Abstractions;

namespace RenStore.Inventory.Tests.Persistence.Integration.Repository.ReservationRepository;

[Collection("sequential")]
public sealed class GetAsyncTests : IAsyncLifetime
{
    private InventoryDbContext _context;
    private readonly ITestOutputHelper testOutputHelper;
    
    public GetAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseNpgsql(connectionString: InventoryRepositoryTestsBase
                .BuildConnectionString(Guid.NewGuid()))
            .Options;

        _context = new InventoryDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }
    
    [Fact]
    public async Task Should_Getting_Reservation_From_Postgres()
    {
        // Arrange
        var quantity1 = 14;
        var now1 = DateTimeOffset.UtcNow;
        var expiresAt1 = DateTimeOffset.UtcNow.AddHours(2);
        var variantId1 = System.Guid.NewGuid();
        var sizeId1 = System.Guid.NewGuid();
        var orderId1 = System.Guid.NewGuid();
        
        var quantity2 = 4;
        var now2 = DateTimeOffset.UtcNow;
        var expiresAt2 = DateTimeOffset.UtcNow.AddHours(2);
        var variantId2 = System.Guid.NewGuid();
        var sizeId2 = System.Guid.NewGuid();
        var orderId2 = System.Guid.NewGuid();

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var reservationRepository = new Inventory.Persistence.Write.Repositories
            .ReservationRepository(eventStore, mediatorMock.Object);
        
        var reservation1 = VariantReservation.Create(
            quantity: quantity1,
            expiresAt: expiresAt1,
            variantId: variantId1,
            sizeId: sizeId1,
            orderId: orderId1,
            now: now1);
        
        var reservation2 = VariantReservation.Create(
            quantity: quantity2,
            expiresAt: expiresAt2,
            variantId: variantId2,
            sizeId: sizeId2,
            orderId: orderId2,
            now: now2);
        
        testOutputHelper.WriteLine($"Reservation 1 ID before save: {reservation1.Id}");
        testOutputHelper.WriteLine($"Reservation 2 ID before save: {reservation2.Id}");
        
        await reservationRepository.SaveAsync(reservation1, CancellationToken.None);
        await reservationRepository.SaveAsync(reservation2, CancellationToken.None);
        
        // Act
        var existingReservation1 = await reservationRepository
            .GetAsync(
                reservation1.Id,
                cancellationToken: CancellationToken.None);
        
        var existingReservation2 = await reservationRepository
            .GetAsync(
                reservation2.Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        Assert.NotNull(existingReservation1);
        Assert.Equal(reservation1.Id, existingReservation1.Id);
        Assert.Equal(quantity1, existingReservation1.Quantity);
        Assert.Equal(variantId1, existingReservation1.VariantId);
        Assert.Equal(sizeId1, existingReservation1.SizeId);
        Assert.Equal(orderId1, existingReservation1.OrderId);

        Assert.NotNull(existingReservation2);
        Assert.Equal(reservation2.Id, existingReservation2.Id);
        Assert.Equal(quantity2, existingReservation2.Quantity);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_ReservationNotFound()
    {
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var reservationRepository = new Inventory.Persistence.Write.Repositories.ReservationRepository(eventStore, mediatorMock.Object);
        
        var result = await reservationRepository.GetAsync(
            Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result); 
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}*/