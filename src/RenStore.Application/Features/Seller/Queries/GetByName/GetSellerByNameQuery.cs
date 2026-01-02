using MediatR;

namespace RenStore.Application.Features.Seller.Queries.GetByName;

public class GetSellerByNameQuery : IRequest<GetSellerByNameVm>
{
    public string Name { get; set; }
}