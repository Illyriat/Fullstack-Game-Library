using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game_Library_Service.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Game"/> entity.
    /// </summary>
    public class GameConfiguration : AuditableEntityConfiguration<Game>
    {
        /// <summary>
        /// Configures the <see cref="Game"/> entity for Entity Framework Core.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(g => g.GenreId)
                .HasDatabaseName("IX_Games_GenreId");

            builder.HasIndex(g => g.PublisherId)
                .HasDatabaseName("IX_Games_PublisherId");
        }
    }
}
