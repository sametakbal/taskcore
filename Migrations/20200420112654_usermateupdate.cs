using Microsoft.EntityFrameworkCore.Migrations;

namespace taskcore.Migrations
{
    public partial class usermateupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccept",
                table: "UserMates");

            migrationBuilder.DropColumn(
                name: "Request",
                table: "UserMates");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "UserMates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "UserMates");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccept",
                table: "UserMates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Request",
                table: "UserMates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
