using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Domain.Repository;

namespace RenStore.Application.Features.Seller.Command.Update;

public class UpdateSellerCommandHandler 
    : IRequestHandler<UpdateSellerCommand>
{
    private readonly ILogger<UpdateSellerCommandHandler> logger;
    private readonly ISellerRepository sellerRepository;

    public UpdateSellerCommandHandler(
        ILogger<UpdateSellerCommandHandler> logger,
        ISellerRepository sellerRepository)
    {
        this.logger = logger;
        this.sellerRepository = sellerRepository;
    }

    public async Task Handle(UpdateSellerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling {nameof(UpdateSellerCommandHandler)}");
        
        var seller = await sellerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        /*if(request.Name != "string")
            seller.Name = request.Name;
        if(request.Description != "string")
            seller.Description = request.Description;
        if(request.ImagePath != "string")
            seller.ImagePath = request.ImagePath;
        if(request.ImageFooterPath != "string")
            seller.ImageFooterPath = request.ImageFooterPath;*/

        await sellerRepository.UpdateAsync(seller, cancellationToken);
        
        logger.LogInformation($"Handled {nameof(UpdateSellerCommandHandler)}");
    }
}