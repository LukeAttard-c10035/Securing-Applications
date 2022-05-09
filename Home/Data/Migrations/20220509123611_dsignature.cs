using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class dsignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "FileTransfer");

            migrationBuilder.AddColumn<string>(
                name: "DigitalSignature",
                table: "FileTransfer",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FileTransfer",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitalSignature",
                table: "FileTransfer");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FileTransfer");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "FileTransfer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
