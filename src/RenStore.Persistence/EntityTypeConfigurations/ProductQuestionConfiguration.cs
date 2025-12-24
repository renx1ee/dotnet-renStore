using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ProductQuestionConfiguration : IEntityTypeConfiguration<ProductQuestionEntity>
{
    public void Configure(EntityTypeBuilder<ProductQuestionEntity> builder)
    {
        builder
            .ToTable("product_questions");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("question_id");

        builder
            .Property(x => x.Message)
            .HasColumnName("message")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .IsRequired();
        
        /*builder
            .Property(x => x.ModeratedDate)
            .HasColumnName("moderated_date")
            .HasDefaultValue(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .IsRequired();*/

        builder
            .Property(x => x.IsApproved)
            .HasColumnName("is_approved")
            .IsRequired(false);

        builder
            .HasOne(x => x.ProductVariant)
            .WithMany(x => x.ProductQuestions)
            .HasForeignKey(x => x.ProductVariantId);

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ProductQuestions)
            .HasForeignKey(x => x.UserId);

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id");

        builder
            .HasOne(x => x.Answer)
            .WithOne(x => x.Question);
    }
}