using MediatR;

namespace RenStore.Application.Features.Seller.Command.Create;

public class CreateSellerCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageFooterPath { get; set; }
}