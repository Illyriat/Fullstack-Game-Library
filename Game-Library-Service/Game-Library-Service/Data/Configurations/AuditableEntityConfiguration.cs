using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game_Library_Service.Data.Configurations
{
    /// <summary>
    /// Base configuration for AuditableEntity properties
    /// This can be used as a base class for entity configurations that inherit from AuditableEntity
    /// </summary>
    public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        /// <summary>
        /// Configures the AuditableEntity base properties for Entity Framework Core.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedDateUtc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Date and time when the entity was created");

            builder.Property(e => e.ModifiedDateUtc)
                .HasComment("Date and time when the entity was last updated");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasComment("User ID who created the entity");

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasComment("User ID who last updated the entity");

            builder.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasComment("Soft delete flag - indicates if the entity has been deleted");

            builder.HasIndex(e => e.CreatedDateUtc)
                .HasDatabaseName($"IX_{typeof(TEntity).Name}_CreatedAt");

            builder.HasIndex(e => e.Deleted)
                .HasDatabaseName($"IX_{typeof(TEntity).Name}_Deleted");

            builder.HasIndex(e => new { e.Deleted, e.CreatedDateUtc })
                .HasDatabaseName($"IX_{typeof(TEntity).Name}_Deleted_CreatedAt");
        }
    }
}
