using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItemEntity>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItemEntity> builder)
    {
        builder
            .ToTable("cart_items");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("cart_item_id");

        builder
            .Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasDefaultValue(1)
            .HasMaxLength(5)
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasColumnName("price")
            .IsRequired();

        builder
            .HasOne(x => x.Cart)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CartId);

        builder
            .Property(x => x.CartId)
            .HasColumnName("cart_id");
        
        builder
            .Property(x => x.ProductId)
            .HasColumnName("product_id");
        
        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.ProductId);
    }
}