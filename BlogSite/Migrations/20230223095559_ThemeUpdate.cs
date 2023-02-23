using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class ThemeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTheme",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Theme",
                table: "Themes");

            migrationBuilder.AddColumn<int>(
                name: "ThemeId",
                table: "Themes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Theme",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Theme",
                table: "Themes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "UK_ThemeName",
                table: "Themes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTheme",
                table: "Posts",
                column: "Theme",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTheme",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Theme",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "UK_ThemeName",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                table: "Themes");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "Posts",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Theme",
                table: "Themes",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTheme",
                table: "Posts",
                column: "Theme",
                principalTable: "Themes",
                principalColumn: "Name",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
