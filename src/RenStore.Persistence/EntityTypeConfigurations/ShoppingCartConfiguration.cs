using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCartEntity>
{
    public void Configure(EntityTypeBuilder<ShoppingCartEntity> builder)
    {
        builder
            .ToTable("shopping_carts");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("cart_id");

        builder
            .Property(x => x.TotalPrice)
            .HasColumnName("total_price")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id");
    }
}