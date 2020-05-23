using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class DbNameAddedToHLFY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DbName",
                table: "HardwareLockFinancialYears",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbName",
                table: "HardwareLockFinancialYears");
        }
    }
}
