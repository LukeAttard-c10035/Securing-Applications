using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class isExpired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isExpired",
                table: "FileTransfer",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isExpired",
                table: "FileTransfer");
        }
    }
}
