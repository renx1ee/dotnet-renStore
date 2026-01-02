using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Domain.Repository;

namespace RenStore.Application.Features.Seller.Command.Create;

public class CreateSellerCommandHandler 
    : IRequestHandler<CreateSellerCommand, int>
{
    private readonly ILogger logger;
    private readonly IMapper mapper;
    private readonly ISellerRepository sellerRepository;

    public CreateSellerCommandHandler(IMapper mapper,
        ILogger<CreateSellerCommandHandler> logger,
        ISellerRepository sellerRepository)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.sellerRepository = sellerRepository;
    }
    
    public async Task<int> Handle(CreateSellerCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling {nameof(CreateSellerCommandHandler)}");
        
        var data = await sellerRepository.FindByNameAsync(request.Name, cancellationToken);
        
        if(data != null)
            return 0;
        
        var seller = mapper.Map<Domain.Entities.SellerEntity>(request);
        
        var result = await sellerRepository.CreateAsync(seller, cancellationToken);
        
        logger.LogInformation($"Handled {nameof(CreateSellerCommandHandler)}");

        return 0;
    }
}