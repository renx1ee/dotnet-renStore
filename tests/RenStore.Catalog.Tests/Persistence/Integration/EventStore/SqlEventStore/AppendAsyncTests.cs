using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Tests.Persistence.Integration.Repositories;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Persistence.Integration.EventStore.SqlEventStore;

public class AppendAsyncTests : IAsyncLifetime
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
    public async Task Should_Save_To_Postgres()
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

        // Act
        Assert.Equal(0, _context.Events.Count());
        
        await store.AppendAsync(
            aggregateId: aggregateId,
            expectedVersion: expectedVersion,
            events: events,
            CancellationToken.None);
        
        // Assert
        Assert.Equal(2, _context.Events.Count());

        var existingEvents = await _context.Events
            .FirstOrDefaultAsync(x =>
                x.AggregateId == aggregateId);
    }
    
    [Fact]
    public async Task Should_Throw_Where_AggregateId_Is_Empty_Guid()
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
        var aggregateId = Guid.Empty;
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

        // Act
        Assert.Equal(0, _context.Events.Count());
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            store.AppendAsync(
                aggregateId: aggregateId,
                expectedVersion: expectedVersion,
                events: events,
                CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Throw_Where_AggregateId_Version_Less_Then_0()
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
        var expectedVersion = -1;

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

        // Act
        Assert.Equal(0, _context.Events.Count());
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            store.AppendAsync(
                aggregateId: aggregateId,
                expectedVersion: expectedVersion,
                events: events,
                CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Throw_Where_Event_Does_Not_Registrated()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var expectedVersion = 0;

        var events = new List<IDomainEvent>()
        {
            new TestEvent(
                EventId: Guid.NewGuid(),
                OccurredAt: now)
        };

        var store = new Catalog.Persistence.EventStore
            .SqlEventStore(_context);

        // Act
        Assert.Equal(0, _context.Events.Count());
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            store.AppendAsync(
                aggregateId: aggregateId,
                expectedVersion: expectedVersion,
                events: events,
                CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Throw_ConcurrencyException_Where_VersionConflict()
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

        var store1 = new Catalog.Persistence.EventStore
            .SqlEventStore(_context);
        
        Assert.Equal(0, _context.Events.Count());

        await store1.AppendAsync(
            aggregateId: aggregateId,
            expectedVersion: expectedVersion,
            events: events,
            CancellationToken.None);
        
        _context.ChangeTracker.Clear();
        
        var store2 = new Catalog.Persistence.EventStore
            .SqlEventStore(_context);
        
        // Act & Assert
        await Assert.ThrowsAsync<ConcurrencyException>(() => 
            store2.AppendAsync(
                aggregateId: aggregateId,
                expectedVersion: expectedVersion,
                events: events,
                CancellationToken.None));
    }
    
    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}

public record TestEvent(
    DateTimeOffset OccurredAt, 
    Guid EventId) 
    : IDomainEvent;