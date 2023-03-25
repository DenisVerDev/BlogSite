using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class PostIndexesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_LPPost",
                table: "LikedPosts");

            migrationBuilder.DropUniqueConstraint(
                name: "UK_PostId",
                table: "Posts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Posts",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_LPPost",
                table: "LikedPosts",
                column:"LikedPost",
                principalTable:"Posts",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddUniqueConstraint(
                name: "UK_PostTitleAuthor",
                table: "Posts",
                columns: new[] { "Title", "Author" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LPPost",
                table: "LikedPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Posts");

            migrationBuilder.DropUniqueConstraint(
                name: "UK_PostTitleAuthor",
                table: "Posts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Posts",
                columns: new[] { "Title", "Author" });

            migrationBuilder.AddUniqueConstraint(
                name: "UK_PostId",
                table: "Posts",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_LPPost",
                table: "LikedPosts",
                column: "LikedPost",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }
    }
}
