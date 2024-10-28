using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalPriceAndPriceFromCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Cart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItem",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Cart",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
