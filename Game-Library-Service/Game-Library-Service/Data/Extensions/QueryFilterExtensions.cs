using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Data.Extensions
{
    /// <summary>
    /// Extension methods for applying global query filters to entities
    /// </summary>
    public static class QueryFilterExtensions
    {
        /// <summary>
        /// Applies global query filters for soft delete to all auditable entities
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure</param>
        public static void ApplyGlobalQueryFilters(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasQueryFilter(g => !g.Deleted);
            modelBuilder.Entity<Publisher>().HasQueryFilter(p => !p.Deleted);
            modelBuilder.Entity<Genre>().HasQueryFilter(g => !g.Deleted);

            // Add new entity query filters here as you create them
        }
    }
}
