using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_Library_Service.Migrations
{
    /// <inheritdoc />
    public partial class ConvertGenreToReferenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing Genre values are free-text and don't map cleanly to the new reference
            // table, so the (dev-only) Games data is wiped rather than converted.
            migrationBuilder.Sql("DELETE FROM [Games];");

            migrationBuilder.DropIndex(
                name: "IX_Games_Genre",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Date and time when the entity was created"),
                    ModifiedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date and time when the entity was last updated"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "User ID who created the entity"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "User ID who last updated the entity"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft delete flag - indicates if the entity has been deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GenreId",
                table: "Games",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_CreatedAt",
                table: "Genres",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Deleted",
                table: "Genres",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Deleted_CreatedAt",
                table: "Genres",
                columns: new[] { "Deleted", "CreatedDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Deleted_Name",
                table: "Genres",
                columns: new[] { "Deleted", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Games_GenreId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Games",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Genre",
                table: "Games",
                column: "Genre");
        }
    }
}
