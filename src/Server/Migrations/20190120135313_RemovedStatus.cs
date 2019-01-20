using Microsoft.EntityFrameworkCore.Migrations;

namespace FestoVideoStream.Migrations
{
    public partial class RemovedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Devices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Devices",
                nullable: false,
                defaultValue: false);
        }
    }
}
