using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FestoVideoStream.Migrations
{
    public partial class PortAndCheckTyp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckType",
                table: "Devices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStreamStatusUpdate",
                table: "Devices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Devices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckType",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastStreamStatusUpdate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Devices");
        }
    }
}
