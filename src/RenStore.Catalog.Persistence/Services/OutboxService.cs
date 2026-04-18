using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Persistence.Outbox;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Persistence.Services;

internal sealed class OutboxService : IOutboxService
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<OutboxService> _logger;

    public OutboxService(
        CatalogDbContext context,
        ILogger<OutboxService> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task Add(
        Guid aggregateId,
        IEnumerable<IDomainEvent> events,
        DateTimeOffset now)
    {
        OutboxWriter.Write(
            context:     _context,
            aggregateId: aggregateId,
            events:      events,
            now:         now);
    }
}