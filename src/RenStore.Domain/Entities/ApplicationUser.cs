using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace RenStore.Domain.Entities;

public class ApplicationUser : IdentityUser, IUser<string>
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string Role { get; set; } 
    public string? Country { get; set; } 
    public string? City { get; set; } 
    public double? Balance { get; set; } 
    public bool? IsActive { get; set; } 
    public DateTime CreatedDate { get; set; }
    public SellerEntity? Seller { get; set; }
    public Guid CartId { get; set; }
    public ShoppingCartEntity? Cart { get; set; }
    public IEnumerable<ReviewEntity>? Reviews { get; set; }
    public IEnumerable<UserImageEntity>? Images { get; set; }
    public IEnumerable<ProductQuestionEntity>? ProductQuestions { get; set; }
    public IEnumerable<ShoppingCartItemEntity>? ShoppingCartItems { get; set; }
    /*public IEnumerable<Order>? Orders { get; set; }*/
}