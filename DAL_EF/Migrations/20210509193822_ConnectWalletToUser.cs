using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class ConnectWalletToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "UserSettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
