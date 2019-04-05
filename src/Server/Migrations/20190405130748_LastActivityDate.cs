using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FestoVideoStream.Migrations
{
    public partial class LastActivityDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                table: "Devices",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStreamingDate",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastStreamingDate",
                table: "Devices");
        }
    }
}
