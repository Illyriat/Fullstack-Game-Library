using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game_Library_Service.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Publisher"/> entity.
    /// </summary>
    public class PublisherConfiguration : AuditableEntityConfiguration<Publisher>
    {
        /// <summary>
        /// Configures the <see cref="Publisher"/> entity for Entity Framework Core.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public override void Configure(EntityTypeBuilder<Publisher> builder)
        {
            base.Configure(builder);

            builder.ToTable("Publishers");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(p => p.Games)
                .WithOne(g => g.Publisher)
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Games_Publishers_PublisherId");

            builder.HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("IX_Publishers_Name");

            builder.HasIndex(p => new { p.Deleted, p.Name })
                .HasDatabaseName("IX_Publishers_Deleted_Name");
        }
    }
}
