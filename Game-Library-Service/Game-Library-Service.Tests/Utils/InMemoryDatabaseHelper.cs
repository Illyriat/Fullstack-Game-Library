using Game_Library_Service.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Tests.Utils
{
    public static class InMemoryDatabaseHelper
    {
        /// <summary>
        /// Creates an ApplicationDbContext backed by a uniquely-named EF Core InMemory database,
        /// so tests don't contaminate each other by sharing state.
        /// </summary>
        public static ApplicationDbContext GetContext(string? databaseName = null)
        {
            databaseName ??= Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
