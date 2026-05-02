using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Persistence;
using RenStore.Delivery.Persistence.EventStore;
using RenStore.Delivery.Persistence.Outbox;

namespace RenStore.Catalog.Persistence.Outbox;

/// <summary>
/// Polls <c>outbox_messages</c> at a fixed interval and publishes
/// each unprocessed message to RabbitMQ via MassTransit.
/// Guarantees at-least-once delivery — consumers must be idempotent.
/// </summary>
internal sealed class OutboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly OutboxOptions _options;
    private readonly ILogger<OutboxWorker> _logger;
    
    public OutboxWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<OutboxOptions> options,
        ILogger<OutboxWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _options      = options.Value;
        _logger       = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "OutboxWorker started. Polling every {Interval}s.",
            _options.PollingIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OutboxWorker encountered an unexpected error.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_options.PollingIntervalSeconds),
                stoppingToken);
        }
        
        _logger.LogInformation("OutboxWorker stopped.");
    }

    private async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        using var scope     = _scopeFactory.CreateScope();
        var context         = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>()!;
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var mediator        = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var messages = await context.OutboxMessages
            .Where(m => 
                m.ProcessedAt == null && 
                m.RetryCount < _options.MaxRetryCount)
            .OrderBy(m => m.CreatedAt)
            .Take(_options.BatchSize)
            .ToListAsync(cancellationToken);
        
        if (messages.Count == 0) return;
        
        _logger.LogDebug("OutboxWorker processing {Count} messages.", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                if (message.Kind == OutboxMessageKind.Domain)
                {
                    var eventType = DomainEventMappings.GetEventType(message.EventType);
                    var @event = JsonSerializer.Deserialize(message.Payload, eventType, EventSerializer.Options);
                    
                    if(@event is null) 
                    {
                        throw new InvalidOperationException(
                            $"Deserialization returned null for event '{message.EventType}'.");
                    }
                    
                    var notificationType = typeof(DomainEventNotification<>)
                        .MakeGenericType(@event.GetType());

                    var notification = (INotification)Activator
                        .CreateInstance(notificationType, @event)!;
                    
                    await mediator.Publish(notification, cancellationToken);
                }
                
                if (message.Kind == OutboxMessageKind.Integration)
                {
                    var eventType = IntegrationEventMappings.GetEventType(message.EventType);
                    var @event = JsonSerializer.Deserialize(message.Payload, eventType, EventSerializer.Options);
                    
                    if(@event is null) 
                    {
                        throw new InvalidOperationException(
                            $"Deserialization returned null for event '{message.EventType}'.");
                    }
                    
                    await publishEndpoint.Publish(@event, eventType, cancellationToken);
                }
                    
                message.ProcessedAt = DateTimeOffset.UtcNow;
                message.Error       = null;
                
                _logger.LogInformation(
                    "Outbox: published {EventType} | MessageId={MessageId} | AggregateId={AggregateId}",
                    message.EventType, 
                    message.Id, 
                    message.AggregateId);
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Error = ex.Message;
                
                _logger.LogWarning(ex,
                    "Outbox: failed to publish {EventType} | MessageId={MessageId} | Attempt={Attempt}",
                    message.EventType, 
                    message.Id, 
                    message.RetryCount);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
}