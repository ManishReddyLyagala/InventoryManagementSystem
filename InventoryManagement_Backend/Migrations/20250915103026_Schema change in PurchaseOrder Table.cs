using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement_Backend.Migrations
{
    /// <inheritdoc />
    public partial class SchemachangeinPurchaseOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesId",
                table: "PurchaseOrders",
                newName: "OrderId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CustomerId",
                table: "PurchaseOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Customers_CustomerId",
                table: "PurchaseOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Customers_CustomerId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_CustomerId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PurchaseOrders",
                newName: "SalesId");
        }
    }
}
