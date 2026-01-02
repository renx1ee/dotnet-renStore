using MediatR;

namespace RenStore.Application.Features.Seller.Command.Delete;

public class DeleteSellerCommand : IRequest
{
    public int Id { get; set; }
}