using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddVisitorsStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitorsStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserAgent = table.Column<string>(maxLength: 1000, nullable: true),
                    UserOs = table.Column<string>(maxLength: 1000, nullable: true),
                    BrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    DeviceName = table.Column<string>(maxLength: 1000, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    PageViewed = table.Column<string>(maxLength: 1000, nullable: true),
                    VisitDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorsStatistics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorsStatistics");
        }
    }
}
