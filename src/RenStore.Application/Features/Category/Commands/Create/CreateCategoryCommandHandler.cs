/*using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Application.Repository;

namespace RenStore.Application.Features.Category.Commands.Create;

public class CreateCategoryCommandHandler 
    : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ILogger logger;
    private readonly IMapper mapper;
    private readonly ICategoryRepository categoryRepository;

    public CreateCategoryCommandHandler(IMapper mapper,
        ILogger<CreateCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.categoryRepository = categoryRepository;
    }
    
    public async Task<int> Handle(CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling {nameof(CreateCategoryCommandHandler)}");

        var data = await categoryRepository.FindByNameAsync(request.Name, cancellationToken);
        
        if(data is not null) return 0;
        
        var category = mapper.Map<Domain.Entities.Category>(request); 
        category.ImagePath = $"categories/{category.Name}";
        
        var result = await categoryRepository.CreateAsync(category, cancellationToken);
        
        logger.LogInformation($"Handled {nameof(CreateCategoryCommandHandler)}");
        
        return result;
    }
}*/