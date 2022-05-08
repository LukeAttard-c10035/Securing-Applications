using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updatedfilesharing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "FileTransfer");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "FileTransfer");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FileTransfer");

            migrationBuilder.AddColumn<string>(
                name: "AuthorizedUsers",
                table: "FileTransfer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizedUsers",
                table: "FileTransfer");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FileTransfer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "FileTransfer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FileTransfer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
