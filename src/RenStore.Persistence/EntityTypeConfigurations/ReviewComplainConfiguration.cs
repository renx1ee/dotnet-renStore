using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ReviewComplainConfiguration : IEntityTypeConfiguration<ReviewComplainEntity>
{
    public void Configure(EntityTypeBuilder<ReviewComplainEntity> builder)
    {
        builder
            .ToTable("review_complains");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("review_complain_id");

        builder
            .Property(x => x.Reason)
            .HasColumnName("reason")
            .IsRequired();
        
        builder
            .Property(x => x.CustomReason)
            .HasColumnName("custom_reason")
            .IsRequired(false);
        
        builder
            .Property(x => x.Comment)
            .HasColumnName("comment")
            .HasMaxLength(1000)
            .IsRequired();

        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasDefaultValue(ReviewComplainStatus.New)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.ResolvedAt)
            .HasColumnName("resolved_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.ModeratorComment)
            .HasColumnName("moderator_comment")
            .HasMaxLength(1000)
            .IsRequired(false);
        
        builder
            .Property(x => x.ModeratorId)
            .HasColumnName("moderator_id")
            .IsRequired(false);

        builder
            .HasOne(x => x.Review)
            .WithMany(x => x.Complains)
            .HasForeignKey(x => x.ReviewId);

        builder
            .Property(x => x.ReviewId)
            .HasColumnName("review_id")
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ReviewComplains)
            .HasForeignKey(x => x.UserId);

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
    }
}