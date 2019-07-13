using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddReferrerToVisitorsStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "VisitorsStatistics",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "VisitorsStatistics");
        }
    }
}
