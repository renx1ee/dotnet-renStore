using MediatR;

namespace RenStore.Application.Features.Seller.Queries.GetById;

public class GetSellerByIdQuery : IRequest<GetSellerByIdVm>
{
    public int Id { get; set; }
}