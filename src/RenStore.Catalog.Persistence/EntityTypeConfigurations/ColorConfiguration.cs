/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder
            .ToTable("colors");
        
        builder
            .HasKey(c => c.Id)
            .HasName("color_id");
        
        builder
            .Property(c => c.Id)
            .HasColumnName("color_id");
        
        builder
            .Property(c => c.Name)
            .HasColumnName("color_name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .HasIndex(c => c.Name)
            .IsUnique();
        
        builder
            .Property(c => c.NormalizedName)
            .HasColumnName("normalized_color_name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .HasIndex(c => c.NormalizedName)
            .IsUnique();
        
        builder
            .Property(c => c.NameRu)
            .HasColumnName("color_name_ru")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .HasIndex(c => c.NameRu)
            .IsUnique();

        builder
            .Property(c => c.ColorCode)
            .HasColumnName("color_code")
            .HasMaxLength(7)
            .IsRequired(false);
        
        builder
            .Property(c => c.Description)
            .HasColumnName("color_description")
            .HasMaxLength(256)
            .IsRequired(false);
    }
}*/