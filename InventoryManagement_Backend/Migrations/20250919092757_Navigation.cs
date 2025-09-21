using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Navigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseSalesOrders_User_UserId",
                table: "PurchaseSalesOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseSalesOrders_User_UserId",
                table: "PurchaseSalesOrders",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseSalesOrders_User_UserId",
                table: "PurchaseSalesOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseSalesOrders_User_UserId",
                table: "PurchaseSalesOrders",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
