using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Belloro.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SalesCount = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId1",
                        column: x => x.CategoryId1,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Lüks Saatler" },
                    { 2, "Klasik Saatler" },
                    { 3, "Spor Saatler" },
                    { 4, "Premium Saatler" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "CategoryId1", "Description", "ImageUrl", "IsFeatured", "IsNew", "Name", "Price", "SalesCount" },
                values: new object[,]
                {
                    { 1, 1, null, "Klasik altın kaplama lüks saat", "/images/products/classic-gold.jpg", true, false, "Belloro Classic Gold", 3450m, 25 },
                    { 2, 1, null, "Zarif rose gold kadın saati", "/images/products/rose-gold.jpg", true, true, "Belloro Rose Gold", 2780m, 18 },
                    { 3, 3, null, "Su geçirmez spor saati", "/images/products/sport.jpg", false, true, "Belloro Sport", 1950m, 32 },
                    { 4, 2, null, "Minimal tasarım klasik saat", "/images/products/minimalist.jpg", false, false, "Belloro Minimalist", 2100m, 15 },
                    { 5, 4, null, "Otomatik mekanizmalı premium saat", "/images/products/automatic.jpg", true, false, "Belloro Automatic", 4280m, 8 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "Amount", "CustomerName", "OrderDate", "OrderNumber", "ProductId", "ProductName", "Status" },
                values: new object[,]
                {
                    { 1, 3450m, "Ahmet Yılmaz", new DateTime(2025, 7, 19, 10, 30, 0, 0, DateTimeKind.Unspecified), "ORD-0025", 1, "Belloro Classic Gold", "Tamamlandı" },
                    { 2, 2780m, "Zeynep Aydın", new DateTime(2025, 7, 20, 14, 15, 0, 0, DateTimeKind.Unspecified), "ORD-0024", 2, "Belloro Rose Gold", "İşlemde" },
                    { 3, 1950m, "Mehmet Kaya", new DateTime(2025, 7, 18, 16, 45, 0, 0, DateTimeKind.Unspecified), "ORD-0023", 3, "Belloro Sport", "Kargoda" },
                    { 4, 2100m, "Ayşe Demir", new DateTime(2025, 7, 16, 11, 20, 0, 0, DateTimeKind.Unspecified), "ORD-0022", 4, "Belloro Minimalist", "Tamamlandı" },
                    { 5, 4280m, "Can Yıldız", new DateTime(2025, 7, 14, 9, 10, 0, 0, DateTimeKind.Unspecified), "ORD-0021", 5, "Belloro Automatic", "Tamamlandı" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId1",
                table: "Products",
                column: "CategoryId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
