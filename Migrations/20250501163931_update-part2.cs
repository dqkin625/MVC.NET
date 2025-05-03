using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBaiBaoKhoaHoc.Migrations
{
    /// <inheritdoc />
    public partial class updatepart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Topics_TopicId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TopicId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "ArticleTopics",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTopics", x => new { x.ArticleId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_ArticleTopics_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTopics_TopicId",
                table: "ArticleTopics",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTopics");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin123");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TopicId",
                table: "Articles",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Topics_TopicId",
                table: "Articles",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
