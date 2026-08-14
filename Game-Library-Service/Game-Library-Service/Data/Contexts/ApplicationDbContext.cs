using Game_Library_Service.Data.Entities;
using Game_Library_Service.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Data.Contexts
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Creates a new instance of the ApplicationDbContext.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Games table
        /// </summary>
        public virtual DbSet<Game> Games { get; set; }

        /// <summary>
        /// Publishers table
        /// </summary>
        public virtual DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Genres table
        /// </summary>
        public virtual DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// Configures the database model by applying entity configurations and global query filters.
        /// </summary>
        /// <param name="builder">The model builder used to configure the database model.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyAllConfigurations();
            builder.ApplyGlobalQueryFilters();
        }

        /// <summary>
        /// Override SaveChanges to automatically set audit fields
        /// </summary>
        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically set audit fields
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Sets audit fields (CreatedDateUtc, ModifiedDateUtc) for entities.
        /// Automatically converts hard deletes to soft deletes for AuditableEntity types.
        /// </summary>
        private void AuditEntities()
        {
            var auditableEntries = ChangeTracker.Entries<AuditableEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            foreach (var entry in auditableEntries)
            {
                var datetime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                // Handle soft delete FIRST - convert hard delete to soft delete
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.Deleted = true;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDateUtc = datetime;
                }

                entry.Entity.ModifiedDateUtc = datetime;
            }
        }
    }
}
