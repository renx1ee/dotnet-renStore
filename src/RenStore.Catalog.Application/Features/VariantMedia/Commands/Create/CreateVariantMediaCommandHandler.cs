using MediatR;
using Microsoft.Extensions.Logging;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Create;

public class CreateVariantMediaCommandHandler
    : IRequestHandler<CreateVariantMediaCommand, Guid>
{
    private readonly ILogger<CreateVariantMediaCommandHandler> _logger;

    public CreateVariantMediaCommandHandler(
        ILogger<CreateVariantMediaCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public Task<Guid> Handle(
        CreateVariantMediaCommand request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}