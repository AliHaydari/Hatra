using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class AddUsefulLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsefulLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Link = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    IsShow = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsefulLinks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsefulLinks");
        }
    }
}
