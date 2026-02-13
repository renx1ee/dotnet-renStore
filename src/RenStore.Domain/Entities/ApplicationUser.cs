using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RenStore.Delivery.Domain.Entities;

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
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public SellerEntity? Seller { get; set; }
    public ShoppingCartEntity? Cart { get; set; }
    public IEnumerable<Address>? Addresses { get; set; }
    public IEnumerable<ReviewEntity>? Reviews { get; set; }
    public IEnumerable<UserImageEntity>? Images { get; set; }
    /*public IEnumerable<ProductQuestionEntity>? ProductQuestions { get; set; }*/
    public IEnumerable<ShoppingCartItemEntity>? ShoppingCartItems { get; set; }
    public IEnumerable<AnswerComplainEntity>? AnswerComplains { get; set; }
    public IEnumerable<QuestionComplainEntity>? QuestionComplains { get; set; }
    /*public IEnumerable<ProductVariantComplainEntity>? ProductVariantComplains { get; set; }*/
    public IEnumerable<ReviewComplainEntity>? ReviewComplains { get; set; }
    public IEnumerable<SellerComplainEntity>? SellerComplains { get; set; }
    // public IEnumerable<PromoCodeUserLimit>? PromoCodeUserLimits { get; set; }
    public IEnumerable<OrderEntity>? Orders { get; set; }

    public void AccountFreeze()
    {
    }
}