using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class RenameOtherWalletId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SourceWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SourceWalletId",
                table: "Transactions",
                newName: "OtherWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SourceWalletId",
                table: "Transactions",
                newName: "IX_Transactions_OtherWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_OtherWalletId",
                table: "Transactions",
                column: "OtherWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_OtherWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "OtherWalletId",
                table: "Transactions",
                newName: "SourceWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_OtherWalletId",
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
    }
}
