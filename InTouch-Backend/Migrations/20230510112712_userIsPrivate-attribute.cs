using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTouch_Backend.Migrations
{
    /// <inheritdoc />
    public partial class userIsPrivateattribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Users");
        }
    }
}
