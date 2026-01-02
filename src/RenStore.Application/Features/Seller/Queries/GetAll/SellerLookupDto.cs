namespace RenStore.Application.Features.Seller.Queries.GetAll;

public class SellerLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public string ImageFooterPath { get; set; }

    public SellerLookupDto(
        int id, 
        string name, 
        string description, 
        string imagePath, 
        string imageFooterPath)
    {
        Id = id;
        Name = name;
        Description = description;
        ImagePath = imagePath;
        ImageFooterPath = imageFooterPath;
    }
}