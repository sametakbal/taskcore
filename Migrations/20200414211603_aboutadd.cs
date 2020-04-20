using Microsoft.EntityFrameworkCore.Migrations;

namespace taskcore.Migrations
{
    public partial class aboutadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Request",
                table: "UserMates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "User",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Request",
                table: "UserMates");

            migrationBuilder.DropColumn(
                name: "About",
                table: "User");
        }
    }
}
