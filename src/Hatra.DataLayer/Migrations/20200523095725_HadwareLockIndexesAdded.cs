using Microsoft.EntityFrameworkCore.Migrations;

namespace Hatra.DataLayer.Migrations
{
    public partial class HadwareLockIndexesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CpuSerialNumber",
                table: "HardwareLocks",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HardwareLocks_LockSerialNumber",
                table: "HardwareLocks",
                column: "LockSerialNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HardwareLocks_LockSerialNumber",
                table: "HardwareLocks");

            migrationBuilder.AlterColumn<string>(
                name: "CpuSerialNumber",
                table: "HardwareLocks",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300);
        }
    }
}
