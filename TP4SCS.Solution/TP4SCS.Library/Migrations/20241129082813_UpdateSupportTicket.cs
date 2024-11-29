using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "SupportTicket",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "SupportTicket");
        }
    }
}
