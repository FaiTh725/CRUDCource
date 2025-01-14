using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Migrations
{
    /// <inheritdoc />
    public partial class Changeorderhistroytocartitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountProduct");

            migrationBuilder.AlterColumn<long>(
                name: "SealerId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AccountId1",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_AccountId1",
                table: "CartItems",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Accounts_AccountId1",
                table: "CartItems",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Accounts_AccountId1",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_AccountId1",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "CartItems");

            migrationBuilder.AlterColumn<long>(
                name: "SealerId",
                table: "Products",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "AccountProduct",
                columns: table => new
                {
                    AccountInShopingCartId = table.Column<long>(type: "bigint", nullable: false),
                    ShopingHistoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProduct", x => new { x.AccountInShopingCartId, x.ShopingHistoryId });
                    table.ForeignKey(
                        name: "FK_AccountProduct_Accounts_AccountInShopingCartId",
                        column: x => x.AccountInShopingCartId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountProduct_Products_ShopingHistoryId",
                        column: x => x.ShopingHistoryId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountProduct_ShopingHistoryId",
                table: "AccountProduct",
                column: "ShopingHistoryId");
        }
    }
}
