using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class VariantImageConfiguration 
    : IEntityTypeConfiguration<VariantImageReadModel>
{
    public void Configure(EntityTypeBuilder<VariantImageReadModel> builder)
    {
        builder
            .ToTable("variant_images");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.OriginalFileName)
            .HasColumnName("original_file_name")
            .HasColumnType("varchar(250)")
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.StoragePath)
            .HasColumnName("storage_path")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.FileSizeBytes)
            .HasColumnName("file_size_bites")
            .HasColumnType("bigint")
            .IsRequired();

        builder
            .Property(x => x.IsMain)
            .HasColumnName("is_main")
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(x => x.SortOrder)
            .HasColumnName("sort_order")
            .IsRequired();

        builder
            .Property(x => x.UploadedAt)
            .HasColumnName("uploaded_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.UpdatedByRole)
            .HasColumnName("updated_by_id")
            .IsRequired();
            
        builder
            .Property(x => x.UpdatedByRole)
            .HasColumnName("updated_by_role")
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(x => x.Weight)
            .HasColumnName("weight")
            .IsRequired();
        
        builder
            .Property(x => x.Height)
            .HasColumnName("height")
            .IsRequired();
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValue("false")
            .IsRequired();
        
        builder
            .Property(x => x.VariantId)
            .HasColumnName("variant_id");

        builder
            .HasIndex(x => new { x.VariantId, x.IsMain })
            .HasDatabaseName("ux_variant_images_variant_id_is_main");
    }
}