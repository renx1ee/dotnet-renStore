using MediatR;

namespace RenStore.Application.Features.Seller.Command.Update;

public class UpdateSellerCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageFooterPath { get; set; }
}