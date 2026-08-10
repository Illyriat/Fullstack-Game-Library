using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_Library_Service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Date and time when the entity was created"),
                    ModifiedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date and time when the entity was last updated"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "User ID who created the entity"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "User ID who last updated the entity"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft delete flag - indicates if the entity has been deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_CreatedAt",
                table: "Games",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Deleted",
                table: "Games",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Deleted_CreatedAt",
                table: "Games",
                columns: new[] { "Deleted", "CreatedDateUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
