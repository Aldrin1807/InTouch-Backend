using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTouch_Backend.Migrations
{
    /// <inheritdoc />
    public partial class userConfirmationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Confirmations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Confirmations_userId",
                table: "Confirmations",
                column: "userId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Confirmations_Users_userId",
                table: "Confirmations",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confirmations_Users_userId",
                table: "Confirmations");

            migrationBuilder.DropIndex(
                name: "IX_Confirmations_userId",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Confirmations");
        }
    }
}
