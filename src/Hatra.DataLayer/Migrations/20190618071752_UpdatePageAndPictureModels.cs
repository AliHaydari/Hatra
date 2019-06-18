using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class UpdatePageAndPictureModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteType",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteUrl",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Picture",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Picture",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "Pages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteType",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "DeleteUrl",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "Pages");
        }
    }
}
