using MediatR;

namespace RenStore.Application.Features.Seller.Queries.GetAll;

public class GetAllSellersListQuery : IRequest<IList<SellerLookupDto>>
{
}