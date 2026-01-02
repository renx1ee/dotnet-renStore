namespace RenStore.Application.Features.Seller.Queries.GetById;

public class GetSellerByIdVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageFooterPath { get; set; }
}