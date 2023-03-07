using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class ThemeRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTheme",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTheme",
                table: "Posts",
                column: "Theme",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTheme",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTheme",
                table: "Posts",
                column: "Theme",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
