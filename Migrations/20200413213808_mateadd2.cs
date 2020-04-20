using Microsoft.EntityFrameworkCore.Migrations;

namespace taskcore.Migrations
{
    public partial class mateadd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    MateId = table.Column<int>(nullable: false),
                    IsAccept = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMates_User_MateId",
                        column: x => x.MateId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserMates_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMates_MateId",
                table: "UserMates",
                column: "MateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMates_UserId",
                table: "UserMates",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMates");
        }
    }
}
