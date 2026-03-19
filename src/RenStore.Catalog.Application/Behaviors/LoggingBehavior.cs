using System.Diagnostics;

namespace RenStore.Catalog.Application.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var stopwatch = Stopwatch.StartNew();
        
        _logger.LogInformation(
            "Handling {RequestName} took {Elapsed}ms",
            requestName,
            stopwatch.ElapsedMilliseconds);

        var response = await next(cancellationToken);
        
        stopwatch.Stop();
        
        _logger.LogInformation(
            "{RequestName} handled in {Elapsed}ms",
            requestName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }
}