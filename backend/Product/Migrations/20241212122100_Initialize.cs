using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SealerId = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_SealerId",
                        column: x => x.SealerId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountProduct",
                columns: table => new
                {
                    AccountInShopingCartId = table.Column<long>(type: "bigint", nullable: false),
                    ShopingCartId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProduct", x => new { x.AccountInShopingCartId, x.ShopingCartId });
                    table.ForeignKey(
                        name: "FK_AccountProduct_Accounts_AccountInShopingCartId",
                        column: x => x.AccountInShopingCartId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountProduct_Products_ShopingCartId",
                        column: x => x.ShopingCartId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_AccountProduct_ShopingCartId",
                table: "AccountProduct",
                column: "ShopingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountProduct1_ShopingHistoryId",
                table: "AccountProduct1",
                column: "ShopingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SealerId",
                table: "Products",
                column: "SealerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountProduct");

            migrationBuilder.DropTable(
                name: "AccountProduct1");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
