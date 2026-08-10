using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_Library_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddGameGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Genre",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Games");
        }
    }
}
