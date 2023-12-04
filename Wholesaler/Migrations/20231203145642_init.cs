using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wholesaler.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoriesDB",
                columns: table => new
                {
                    Product_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    Manufacturer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer_ref_num = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shipping = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shipping_cost = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoriesDB", x => x.Product_id);
                });

            migrationBuilder.CreateTable(
                name: "PricesDB",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nett_product_price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nett_product_price_discount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vat_Tax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nett_product_price_discount_logistic_unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsDB",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Can_be_returned = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Producer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_wire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shipping = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Package_size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logistic_height = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logistic_width = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logistic_length = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logistic_weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available_in_parcel_locker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Default_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDB", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoriesDB_SKU",
                table: "InventoriesDB",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PricesDB_SKU",
                table: "PricesDB",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDB_SKU",
                table: "ProductsDB",
                column: "SKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoriesDB");

            migrationBuilder.DropTable(
                name: "PricesDB");

            migrationBuilder.DropTable(
                name: "ProductsDB");
        }
    }
}
