using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTouch_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FollowRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowRequests",
                columns: table => new
                {
                    FollowRequestId = table.Column<int>(type: "int", nullable: false),
                    FollowRequestedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowRequests", x => new { x.FollowRequestId, x.FollowRequestedId });
                    table.ForeignKey(
                        name: "FK_FollowRequests_Users_FollowRequestId",
                        column: x => x.FollowRequestId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowRequests_Users_FollowRequestedId",
                        column: x => x.FollowRequestedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_FollowRequestedId",
                table: "FollowRequests",
                column: "FollowRequestedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowRequests");
        }
    }
}
