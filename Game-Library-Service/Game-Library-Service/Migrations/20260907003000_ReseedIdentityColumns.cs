using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_Library_Service.Migrations
{
    /// <inheritdoc />
    public partial class ReseedIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Identity caching (see DisableIdentityCache) let these tables' identity counters
            // jump far ahead of their actual max Id before caching was turned off. RESEED with
            // no explicit value only corrects a counter that's too LOW, so the target value is
            // computed per-table and passed explicitly to also correct counters sitting too high.
            migrationBuilder.Sql(ReseedSql("Games"));
            migrationBuilder.Sql(ReseedSql("Publishers"));
            migrationBuilder.Sql(ReseedSql("Genres"));
        }

        private static string ReseedSql(string tableName) => $@"
DECLARE @maxId int;
SELECT @maxId = ISNULL(MAX(Id), 0) FROM [{tableName}];
DBCC CHECKIDENT ('{tableName}', RESEED, @maxId);";

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Not reversible: the prior (gapped) identity value isn't recoverable.
        }
    }
}
