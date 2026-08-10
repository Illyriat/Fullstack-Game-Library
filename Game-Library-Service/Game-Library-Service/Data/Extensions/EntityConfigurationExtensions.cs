using Game_Library_Service.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Data.Extensions
{
    /// <summary>
    /// Extension methods for applying entity configurations to the model builder
    /// </summary>
    public static class EntityConfigurationExtensions
    {
        /// <summary>
        /// Applies all entity configurations to the model builder
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure</param>
        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());

            // Add new entity configurations here as you create them
        }
    }
}
