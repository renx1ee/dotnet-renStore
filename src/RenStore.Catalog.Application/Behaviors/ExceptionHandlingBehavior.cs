using System.Data;

namespace RenStore.Catalog.Application.Behaviors;

internal sealed class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(
        ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger) =>
        _logger = logger;
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (DomainException e)
        {
            _logger.LogWarning(
                e,
                message: "Domain rule violated in {RequestName}: {Message}",
                typeof(TRequest).Name,
                e.Message);
            throw;
        }
        catch (DataException e)
        {
            _logger.LogError(
                e,
                message: "Infrastructure error in {RequestName}: {Message}",
                typeof(TRequest).Name,
                e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                message: "Unhandled error in {RequestName}: {Message}",
                typeof(TRequest).Name,
                e.Message);
            throw;
        }
    }
}