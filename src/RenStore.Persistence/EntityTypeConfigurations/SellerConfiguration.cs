using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class SellerConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder
            .ToTable("sellers");
        
        builder
            .HasKey(seller => seller.Id);
        
        builder
            .Property(seller => seller.Id)
            .HasColumnName("seller_id");

        builder
            .Property(seller => seller.Name)
            .HasMaxLength(50)
            .HasColumnName("seller_name")
            .IsRequired();
        
        builder
            .HasIndex(seller => seller.Name)
            .IsUnique();
        
        builder
            .Property(seller => seller.NormalizedName)
            .HasMaxLength(50)
            .HasColumnName("normalized_seller_name")
            .IsRequired();
        
        builder
            .HasIndex(seller => seller.NormalizedName)
            .IsUnique();
        
        builder
            .Property(seller => seller.Description)
            .HasMaxLength(500)
            .HasColumnName("seller_description")
            .IsRequired(false);
        
        builder
            .Property(seller => seller.CreatedAt)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
        
        builder
            .Property(seller => seller.IsBlocked)
            .HasColumnName("is_blocked")
            .IsRequired();
        
        builder
            .Property(v => v.Url)
            .HasColumnName("url")
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(seller => seller.ApplicationUserId)
            .HasColumnName("user_id")
            .IsRequired();
    }
}