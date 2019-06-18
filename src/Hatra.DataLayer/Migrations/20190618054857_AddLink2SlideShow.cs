using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddLink2SlideShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "SlideShow",
                newName: "Link2");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SlideShow",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Link1",
                table: "SlideShow",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link1",
                table: "SlideShow");

            migrationBuilder.RenameColumn(
                name: "Link2",
                table: "SlideShow",
                newName: "Link");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SlideShow",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
