using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceandCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Service_CategoryId",
                table: "Service",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Service__Category__6112A204",
                table: "Service",
                column: "CategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Service__Category__6112A204",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_CategoryId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Service");
        }
    }
}
