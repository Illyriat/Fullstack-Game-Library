using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game_Library_Service.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Genre"/> entity.
    /// </summary>
    public class GenreConfiguration : AuditableEntityConfiguration<Genre>
    {
        /// <summary>
        /// Configures the <see cref="Genre"/> entity for Entity Framework Core.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public override void Configure(EntityTypeBuilder<Genre> builder)
        {
            base.Configure(builder);

            builder.ToTable("Genres");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(g => g.Games)
                .WithOne(game => game.Genre)
                .HasForeignKey(game => game.GenreId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Games_Genres_GenreId");

            builder.HasIndex(g => g.Name)
                .IsUnique()
                .HasDatabaseName("IX_Genres_Name");

            builder.HasIndex(g => new { g.Deleted, g.Name })
                .HasDatabaseName("IX_Genres_Deleted_Name");
        }
    }
}
