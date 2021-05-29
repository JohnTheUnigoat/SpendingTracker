using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_EF.Migrations
{
    public partial class AddWalletAllowedUserManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletAllowedUser",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletAllowedUser", x => new { x.WalletId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WalletAllowedUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletAllowedUser_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletAllowedUser_UserId",
                table: "WalletAllowedUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletAllowedUser");
        }
    }
}
