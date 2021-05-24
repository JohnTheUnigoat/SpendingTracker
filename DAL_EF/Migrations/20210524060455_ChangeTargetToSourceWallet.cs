using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class ChangeTargetToSourceWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_TargetWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TargetWalletId",
                table: "Transactions",
                newName: "SourceWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_TargetWalletId",
                table: "Transactions",
                newName: "IX_Transactions_SourceWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_SourceWalletId",
                table: "Transactions",
                column: "SourceWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SourceWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SourceWalletId",
                table: "Transactions",
                newName: "TargetWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SourceWalletId",
                table: "Transactions",
                newName: "IX_Transactions_TargetWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_TargetWalletId",
                table: "Transactions",
                column: "TargetWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
