/*using SizeSystem.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RenStore.Application.Queues;
using RenStore.Application.Services;

namespace RenStore.Application.BackgroundServices;

public class PriceCounterBackgroundService : BackgroundService
{
    private readonly ILogger<PriceCounterBackgroundService> logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IProductRatingQueue productRatingQueue;
    private readonly IConfiguration configuration;

    public PriceCounterBackgroundService(
        ILogger<PriceCounterBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IProductRatingQueue productRatingQueue,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
        this.configuration = configuration;
        this.productRatingQueue = productRatingQueue;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Product rating recalculation method is starting.");
        
        await foreach (var productId in ReadQueueAsync(stoppingToken))
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var productService = scope.ServiceProvider.GetRequiredService<ProductService>();

                await productService.CalculateProductRatingAsync(productId, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error recalculating rating for product: {productId}, message {ex}");
            }
        }
        
        logger.LogInformation("Product rating recalculation method is stopping.");
    }

    private async IAsyncEnumerable<Guid> ReadQueueAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return await productRatingQueue.DequeueAsync(cancellationToken);
        }
    }
}*/