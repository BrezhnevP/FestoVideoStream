using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FestoVideoStream.Migrations
{
    public partial class LastStreamDate_into_Start_and_End_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStreamEndDate",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStreamEndDate",
                table: "Devices");
        }
    }
}
