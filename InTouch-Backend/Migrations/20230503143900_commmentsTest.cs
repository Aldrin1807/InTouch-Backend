using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTouch_Backend.Migrations
{
    /// <inheritdoc />
    public partial class commmentsTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "Id", "UserId", "PostId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "UserId", "PostId", "Id" });
        }
    }
}
