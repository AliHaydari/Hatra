using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddIAuditableEntityToFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByBrowserName",
                table: "Folders",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByIp",
                table: "Folders",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDateTime",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByBrowserName",
                table: "Folders",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByIp",
                table: "Folders",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Folders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByBrowserName",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CreatedByIp",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedByBrowserName",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedByIp",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Folders");
        }
    }
}
