using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_Library_Service.Migrations
{
    /// <inheritdoc />
    public partial class DisableIdentityCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server caches a block of identity values in memory (1000 for int columns) and
            // discards whatever is unused if the server restarts, causing the next insert to jump
            // ahead. Disabling the cache trades a little insert throughput for gap-free IDs.
            migrationBuilder.Sql("ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;", suppressTransaction: true);
        }
    }
}
