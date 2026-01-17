using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder
            .ToTable("reviews");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("review_id");

        builder
            .Property(x => x.Message)
            .HasColumnName("message")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.ReviewRating)
            .HasColumnName("rating")
            .HasDefaultValue(0)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
        
        builder
            .Property(x => x.LastUpdatedDate)
            .HasColumnName("last_updated_date")
            .IsRequired(false);

        builder
            .Property(x => x.IsUpdated)
            .HasColumnName("is_updated")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder
            .Property(x => x.ModeratedDate)
            .HasColumnName("moderated_date")
            .IsRequired(false);

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.IsApproved)
            .HasColumnName("is_approved")
            .HasDefaultValue(null)
            .IsRequired();

        builder
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId);

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id");
        
        /*builder
            .HasOne(x => x.ProductVariant)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductVariantId);*/

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}