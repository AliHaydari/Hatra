using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddIAuditableEntityToPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByBrowserName",
                table: "Pictures",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByIp",
                table: "Pictures",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDateTime",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByBrowserName",
                table: "Pictures",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByIp",
                table: "Pictures",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByBrowserName",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedByIp",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedByBrowserName",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedByIp",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Pictures");
        }
    }
}
