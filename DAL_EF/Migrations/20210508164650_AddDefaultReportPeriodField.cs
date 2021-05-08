using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class AddDefaultReportPeriodField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultReportPeriod",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Current month");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultReportPeriod",
                table: "Wallets");
        }
    }
}
