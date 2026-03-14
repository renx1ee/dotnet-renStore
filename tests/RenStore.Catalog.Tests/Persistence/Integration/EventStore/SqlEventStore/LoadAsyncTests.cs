using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Tests.Persistence.Integration.Repositories;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Tests.Persistence.Integration.EventStore.SqlEventStore;

public class LoadAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: CatalogRepositoryTestsBase
                .BuildConnectionString(Guid.NewGuid()))
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Should_Load_Events_In_Order()
    {
        // Arrange
        var now1 = DateTimeOffset.UtcNow;
        var now2 = DateTimeOffset.UtcNow.AddSeconds(1);
        var key1 = "Test key1";
        var key2 = "Test key2";
        var value1 = "Test value1";
        var value2 = "Test value2";
        var variantId = Guid.NewGuid();
        var attributeId = Guid.NewGuid();
        var aggregateId = Guid.NewGuid();
        var expectedVersion = 0;

        var events = new List<IDomainEvent>()
        {
            new AttributeCreatedEvent(
                EventId: Guid.NewGuid(),
                OccurredAt: now1,
                VariantId: variantId,
                AttributeId: attributeId,
                Key: key1,
                Value: value1),
            
            new AttributeCreatedEvent(
                EventId: Guid.NewGuid(),
                OccurredAt: now2,
                VariantId: variantId,
                AttributeId: attributeId,
                Key: key2,
                Value: value2),
        };

        var store = new Catalog.Persistence.EventStore
            .SqlEventStore(_context);
        
        Assert.Equal(0, _context.Events.Count());
        
        await store.AppendAsync(
            aggregateId: aggregateId,
            expectedVersion: expectedVersion,
            events: events,
            CancellationToken.None);
        
        // Act
        var result = await store.LoadAsync(
            aggregateId, 
            CancellationToken.None);
        
        Assert.NotNull(result);
        
        // Assert: first
        Assert.Equal(now1, result[0].OccurredAt);
        Assert.IsType<AttributeCreatedEvent>(result[0]);
        Assert.Equal(key1, ((AttributeCreatedEvent)result[0]).Key);
        Assert.Equal(value1, ((AttributeCreatedEvent)result[0]).Value);
        
        // Assert: second
        Assert.Equal(now2, result[1].OccurredAt);
        Assert.IsType<AttributeCreatedEvent>(result[1]);
        Assert.Equal(key2, ((AttributeCreatedEvent)result[1]).Key);
        Assert.Equal(value2, ((AttributeCreatedEvent)result[1]).Value);
    }
    
    [Fact]
    public async Task Should_Throw_When_Aggregate_Id_Is_Null()
    {
        // Arrange
        var now1 = DateTimeOffset.UtcNow;
        var now2 = DateTimeOffset.UtcNow.AddSeconds(1);
        var key1 = "Test key1";
        var key2 = "Test key2";
        var value1 = "Test value1";
        var value2 = "Test value2";
        var variantId = Guid.NewGuid();
        var attributeId = Guid.NewGuid();
        var aggregateId = Guid.NewGuid();
        var wrongAggregateId = Guid.Empty;
        var expectedVersion = 0;

        var events = new List<IDomainEvent>()
        {
            new AttributeCreatedEvent(
                EventId: Guid.NewGuid(),
                OccurredAt: now1,
                VariantId: variantId,
                AttributeId: attributeId,
                Key: key1,
                Value: value1),
            
            new AttributeCreatedEvent(
                EventId: Guid.NewGuid(),
                OccurredAt: now2,
                VariantId: variantId,
                AttributeId: attributeId,
                Key: key2,
                Value: value2),
        };

        var store = new Catalog.Persistence.EventStore
            .SqlEventStore(_context);
        
        Assert.Equal(0, _context.Events.Count());
        
        await store.AppendAsync(
            aggregateId: aggregateId,
            expectedVersion: expectedVersion,
            events: events,
            CancellationToken.None);
        
        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            store.LoadAsync(wrongAggregateId, CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}