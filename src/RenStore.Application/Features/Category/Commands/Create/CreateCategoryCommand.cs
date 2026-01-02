using MediatR;

namespace RenStore.Application.Features.Category.Commands.Create;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
}