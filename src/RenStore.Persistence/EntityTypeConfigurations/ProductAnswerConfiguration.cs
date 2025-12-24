/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ProductAnswerConfiguration : IEntityTypeConfiguration<ProductAnswerEntity>
{
    public void Configure(EntityTypeBuilder<ProductAnswerEntity> builder)
    {
        builder
            .ToTable("product_answers");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("answer_id");

        builder
            .Property(x => x.Message)
            .HasColumnName("message")
            .HasMaxLength(500)
            .IsRequired();
        
        
        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue( DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .IsRequired();
        
        /*builder
            .Property(x => x.ModeratedDate)
            .HasColumnName("moderated_date")
            .HasDefaultValue( DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .IsRequired(false);#1#
        
        builder
            .Property(x => x.IsApproved)
            .HasColumnName("is_approved")
            .IsRequired(false);
        
        builder
            .HasOne(x => x.Seller)
            .WithMany(x => x.ProductAnswers)
            .HasForeignKey(x => x.SellerId);

        builder
            .Property(x => x.SellerId)
            .HasColumnName("seller_id");
        
        builder
            .HasOne(x => x.Question)
            .WithOne(x => x.Answer)
            .HasForeignKey<ProductAnswerEntity>(x => x.QuestionId);

        builder
            .Property(x => x.QuestionId)
            .HasColumnName("question_id");


    }
}*/