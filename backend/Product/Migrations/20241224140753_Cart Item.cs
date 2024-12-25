using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Migrations
{
    /// <inheritdoc />
    public partial class CartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountProduct_Products_ShopingCartId",
                table: "AccountProduct");

            migrationBuilder.DropTable(
                name: "AccountProduct1");

            migrationBuilder.RenameColumn(
                name: "ShopingCartId",
                table: "AccountProduct",
                newName: "ShopingHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountProduct_ShopingCartId",
                table: "AccountProduct",
                newName: "IX_AccountProduct_ShopingHistoryId");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_AccountId",
                table: "CartItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountProduct_Products_ShopingHistoryId",
                table: "AccountProduct",
                column: "ShopingHistoryId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountProduct_Products_ShopingHistoryId",
                table: "AccountProduct");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ShopingHistoryId",
                table: "AccountProduct",
                newName: "ShopingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountProduct_ShopingHistoryId",
                table: "AccountProduct",
                newName: "IX_AccountProduct_ShopingCartId");

            migrationBuilder.CreateTable(
                name: "AccountProduct1",
                columns: table => new
                {
                    AccountInShopingHistoryId = table.Column<long>(type: "bigint", nullable: false),
                    ShopingHistoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProduct1", x => new { x.AccountInShopingHistoryId, x.ShopingHistoryId });
                    table.ForeignKey(
                        name: "FK_AccountProduct1_Accounts_AccountInShopingHistoryId",
                        column: x => x.AccountInShopingHistoryId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountProduct1_Products_ShopingHistoryId",
                        column: x => x.ShopingHistoryId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountProduct1_ShopingHistoryId",
                table: "AccountProduct1",
                column: "ShopingHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountProduct_Products_ShopingCartId",
                table: "AccountProduct",
                column: "ShopingCartId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
