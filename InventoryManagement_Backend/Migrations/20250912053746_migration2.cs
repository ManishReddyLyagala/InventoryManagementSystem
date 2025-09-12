using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement_Backend.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "EmailID",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MobileNumber",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailID",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
