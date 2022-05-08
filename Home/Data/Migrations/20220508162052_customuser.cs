using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class customuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "privateKey",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "publicKey",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "privateKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "publicKey",
                table: "AspNetUsers");
        }
    }
}
