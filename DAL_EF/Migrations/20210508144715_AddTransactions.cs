using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class AddTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Wallets_WalletId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceWalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    TargetWalletId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_SourceWalletId",
                        column: x => x.SourceWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_TargetWalletId",
                        column: x => x.TargetWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceWalletId",
                table: "Transactions",
                column: "SourceWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TargetWalletId",
                table: "Transactions",
                column: "TargetWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Wallets_WalletId",
                table: "Categories",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "UserSettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Wallets_WalletId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "GoogleId", "LastName", "PictureUrl" },
                values: new object[] { 1, "asdf@asdf.com", "John", "asdf", "Smith", "no" });

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Wallets_WalletId",
                table: "Categories",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_UserSettings_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "UserSettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
